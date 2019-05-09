/*
 *
 * HomePage actions
 *
 */

import {
  GET_USERS,
  SET_USERS,
  GET_USERS_ERROR,
  FIND_USERS,
  FIND_USERS_SUCCESS,
  FIND_USERS_ERROR,
  ADD_USER,
  ADD_USER_SUCCESS,
  ADD_USER_ERROR,
  GET_DATA,
  GET_DATA_SUCCESS,
  GET_DATA_ERROR,
  GET_FRIENDS,
  SET_FRIENDS,
  GET_FRIENDS_ERROR,
  SET_USERS_COUNT,
  USERNAME_CHANGED,
  GET_USER_INFO_ERROR,
  SET_USER_INFO,
  GET_USER_INFO,
} from './constants';

export const getUsers = () => ({ type: GET_USERS });
export const setUsers = users => ({ type: SET_USERS, users });
export const getUsersError = error => ({ type: GET_USERS_ERROR, error });

export const findUsers = text => ({ type: FIND_USERS, text });
export const findUsersSuccess = data => ({ type: FIND_USERS_SUCCESS, data });
export const findUsersError = error => ({ type: FIND_USERS_ERROR, error });

export const addUser = id => ({ type: ADD_USER, id });
export const addUserSuccess = () => ({ type: ADD_USER_SUCCESS });
export const addUserError = error => ({ type: ADD_USER_ERROR, error });

export const getData = (id, from, to) => ({ type: GET_DATA, id, from, to });
export const setData = data => ({ type: GET_DATA_SUCCESS, data });
export const getDataError = error => ({ type: GET_DATA_ERROR, error });

export const getFriends = userId => ({ type: GET_FRIENDS, userId });
export const setFriends = data => ({ type: SET_FRIENDS, data });
export const getFriendsError = error => ({ type: GET_FRIENDS_ERROR, error });

export const setUsersCount = count => ({ type: SET_USERS_COUNT, count });

export const usernameChanged = name => ({ type: USERNAME_CHANGED, name });

export const getUserInfo = id => ({ type: GET_USER_INFO, id });
export const setUserInfo = info => ({ type: SET_USER_INFO, info });
export const getUserInfoError = error => ({ type: GET_USER_INFO_ERROR, error });
