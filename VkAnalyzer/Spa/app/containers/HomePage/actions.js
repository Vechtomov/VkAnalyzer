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
