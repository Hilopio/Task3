#include "pch.h"
#include <mkl.h>
#include <mkl_df_defines.h>
#include <iostream>
#include <vector>
using namespace std;

MKL_INT s_order = DF_PP_CUBIC;
MKL_INT s_type = DF_PP_NATURAL;
MKL_INT bc_type = DF_BC_FREE_END;
enum class ErrorCode { NO, INIT_ERROR, CHECK_ERROR, SOLVE_ERROR, JACOBI_ERROR, GET_ERROR, DELETE_ERROR, RCI_ERROR };

struct UserData
{
    double* borders;
    double* X;
    double* Y;
    int n;
    double* SplineValues;
    bool residual;
};

void SplineMaking(MKL_INT* n, MKL_INT* m, double* valuesOnUniformGrid, double* f, void* user_data)
{
    try {
        UserData d = *((UserData*)user_data);
        // creating task
        MKL_INT status;
        DFTaskPtr task = nullptr;
        status = dfdNewTask1D(&task, *m, d.borders, DF_UNIFORM_PARTITION, 1, valuesOnUniformGrid, DF_NO_HINT);
        if (status != DF_STATUS_OK) throw "Can not create task";

        // creating spline interpolation
        status = dfdEditPPSpline1D(task, s_order, s_type, bc_type, nullptr, DF_NO_IC, nullptr, d.SplineValues, DF_NO_HINT);
        if (status != DF_STATUS_OK) throw "Can not create spline interpolation";

        // constructing spline from task
        status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
        if (status != DF_STATUS_OK) throw "Can not construct spline from task";



        // non-uniform interpolation of function
        int dorder = 1;
        double* Spline = new double[*n];
        status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, d.n, d.X, DF_NON_UNIFORM_PARTITION, 1, &dorder, nullptr, Spline, DF_NO_HINT, nullptr);
        if (status != DF_STATUS_OK) throw "Can not interpolate function.";


        for (int i = 0; i < d.n; ++i) {
            if (d.residual) f[i] = pow((Spline[i] - d.Y[i]), 2);
            else f[i] = Spline[i];
        }

        // deleting task
        status = dfDeleteTask(&task);
        if (status != DF_STATUS_OK) throw "Can not delete task";

        delete[] Spline;
    }
    catch (const char* message) {
        cout << message << endl;
    }
}

extern "C" _declspec(dllexport)
void Interpolation(int n, double* X, double* Y, int m, double* StartValues, double* SplineValues,
    int* StopInfo, int IterationsLimit, int* Iterations)
{
    double* _SplineValues = new double[1 * (n - 1) * s_order];
    int status = -1;

    MKL_INT maxOuterIterations = IterationsLimit;
    MKL_INT maxInnerIterations = 10;
    MKL_INT doneIterations = 0;
    double rho = 10;

    const double eps[] = {
        1.0E-12,
        1.0E-12,
        1.0E-12,
        1.0E-12,
        1.0E-12,
        1.0E-12
    };

    double jacobianEps = 1.0E-10;

    double initialResidual = 0;
    double finalResidual = 0;
    MKL_INT terminationReason;
    MKL_INT checkDataInfo[4];
    ErrorCode error = ErrorCode(ErrorCode::NO);

    _TRNSP_HANDLE_t handle = nullptr;
    double* Residual = nullptr;
    double* Jacobian = nullptr;

    try
    {
        double boundaries[2] = { X[0], X[n - 1] };

        Residual = new double[n];
        Jacobian = new double[n * m];

        MKL_INT status = dtrnlsp_init(&handle, &m, &n, StartValues, eps, &maxOuterIterations, &maxInnerIterations, &rho);
        if (status != TR_SUCCESS) throw (ErrorCode(ErrorCode::INIT_ERROR));

        status = dtrnlsp_check(&handle, &m, &n, Jacobian, Residual, eps, checkDataInfo);
        if (status != TR_SUCCESS) throw (ErrorCode(ErrorCode::CHECK_ERROR));

        MKL_INT rciRequest = 0;

        UserData d;
        d.n = n;
        d.X = X;
        d.Y = Y;
        d.borders = boundaries;
        d.SplineValues = _SplineValues;
        d.residual = true;

        bool skipSplineConstruction = false;
        while (true)
        {
            if (!skipSplineConstruction) {
                skipSplineConstruction = true;
            }

            status = dtrnlsp_solve(&handle, Residual, Jacobian, &rciRequest);
            if (status != TR_SUCCESS) throw (ErrorCode(ErrorCode::SOLVE_ERROR));

            if (rciRequest == 0) continue;
            else if (rciRequest == 1) SplineMaking(&n, &m, StartValues, Residual, static_cast<void*>(&d));
            else if (rciRequest == 2)
            {
                status = djacobix(SplineMaking, &m, &n, Jacobian, StartValues, &jacobianEps, static_cast<void*>(&d));
                if (status != TR_SUCCESS) throw (ErrorCode(ErrorCode::JACOBI_ERROR));
            }
            else if (rciRequest >= -6 && rciRequest <= -1) break;
            else throw (ErrorCode(ErrorCode::RCI_ERROR));
        }

        status = dtrnlsp_get(&handle, &doneIterations, &terminationReason, &initialResidual, &finalResidual);
        if (status != TR_SUCCESS) throw (ErrorCode(ErrorCode::GET_ERROR));

        status = dtrnlsp_delete(&handle);
        if (status != TR_SUCCESS) throw (ErrorCode(ErrorCode::DELETE_ERROR));
        d.residual = false;
        SplineMaking(&n, &m, StartValues, SplineValues, static_cast<void*>(&d));

        *StopInfo = terminationReason;
        *Iterations = doneIterations;

    }
    catch (ErrorCode _error) { error = _error; }
    catch (const char* message)
    {
        cout << message << endl;
    }

    if (Residual != nullptr) delete[] Residual;
    if (Jacobian != nullptr) delete[] Jacobian;

    delete[] _SplineValues;
}
