import { call, put, takeLatest } from 'redux-saga/effects';
import { GET_USERS, FIND_USERS, ADD_USER } from './constants';
import {
  setUsers,
  getUsersError,
  findUsersSuccess,
  findUsersError,
  addUserSuccess,
  addUserError,
} from './actions';
import UsersService from '../../services/users';

export default function* defaultSaga() {
  yield takeLatest(GET_USERS, getUsersFlow);
  yield takeLatest(FIND_USERS, findUsersFlow);
  yield takeLatest(ADD_USER, addUserFlow);
}

function* getUsersFlow() {
  try {
    const response = yield call(UsersService.getUsers);
    if (response.success) {
      yield put(setUsers(response.data));
    } else {
      yield put(getUsersError(response.errorMessage));
    }
  } catch (error) {
    yield put(getUsersError(error.message));
  }
}

function* findUsersFlow({ text }) {
  try {
    const response = yield call(UsersService.findUsers, text);
    if (response.success) {
      yield put(findUsersSuccess(response.data));
    } else {
      yield put(findUsersError(response.errorMessage));
    }
  } catch (error) {
    yield put(findUsersError(error.message));
  }
}

function* addUserFlow({ id }) {
  try {
    const response = yield call(UsersService.findUsers, [id]);
    if (response.success) {
      yield put(addUserSuccess());
    } else {
      yield put(addUserError(response.errorMessage));
    }
  } catch (error) {
    yield put(addUserError(error.message));
  }
}