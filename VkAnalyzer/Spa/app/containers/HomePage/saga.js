import { call, put, takeLatest, fork, take, cancel } from 'redux-saga/effects';
import { delay } from 'redux-saga';
import {
  GET_USERS,
  FIND_USERS,
  ADD_USER,
  GET_DATA,
  GET_FRIENDS,
} from './constants';
import {
  setUsers,
  getUsersError,
  findUsersSuccess,
  findUsersError,
  addUserSuccess,
  addUserError,
  setData,
  getDataError,
  setFriends,
  getFriendsError,
  setUsersCount,
  setUserInfo,
  getUserInfoError,
} from './actions';
import UsersService from '../../services/users';
import { setError } from '../App/actions';

export default function* defaultSaga() {
  yield fork(commonRequests);
  yield fork(watchInput);
}

function* commonRequests() {
  yield takeLatest(GET_USERS, getUsersFlow);
  yield takeLatest(ADD_USER, addUserFlow);
  yield takeLatest(GET_DATA, getOnlineDataFlow);
  yield takeLatest(GET_FRIENDS, getFriendsFlow);
}

function* watchInput() {
  let task;
  while (true) {
    const { text } = yield take(FIND_USERS);
    if (task) {
      yield cancel(task);
    }
    task = yield fork(findUsersFlow, text);
  }
}

function* findUsersFlow(text) {
  try {
    yield delay(2000);
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

function* getUsersFlow() {
  try {
    const { success, data } = yield call(UsersService.getUsersCount);
    if (success) yield put(setUsersCount(data));

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

function* addUserFlow({ id }) {
  try {
    const response = yield call(UsersService.addUsers, [id]);
    if (response.success) {
      yield put(addUserSuccess());
      yield call(getUsersFlow);
    } else {
      yield put(addUserError(response.errorMessage));
    }
  } catch (error) {
    yield put(addUserError(error.message));
  }
}

function* getOnlineDataFlow({ id, from, to }) {
  try {
    const userResponse = yield call(UsersService.getUserInfo, id);
    if (userResponse.success) {
      yield put(setUserInfo(userResponse.data));
    } else {
      yield put(getUserInfoError(userResponse.errorMessage));
    }

    const response = yield call(UsersService.getData, id, from, to);
    if (response.success) {
      yield put(setData(response.data));
    } else {
      yield put(getDataError(response.errorMessage));
    }
  } catch (error) {
    yield put(getDataError(error.message));
  }
}

function* getFriendsFlow({ userId }) {
  try {
    const response = yield call(UsersService.getFriends, userId);
    const { success, data } = response;
    if (success) {
      yield put(setError(`Друзей: ${data.length}`));
      yield put(setFriends(data));
      yield call(UsersService.addUsers, data.map(i => i.id));
      if (data.length === 0) yield put(setError('Друзей не найдено'));
    } else {
      yield put(getFriendsError(response.errorMessage));
    }
  } catch (error) {
    yield put(setError(error.message));
  }
}
