/*
 *
 * HomePage reducer
 *
 */

import { fromJS } from 'immutable';
import {
  GET_USERS,
  SET_USERS,
  GET_USERS_ERROR,
  FIND_USERS_SUCCESS,
  FIND_USERS_ERROR,
  ADD_USER_SUCCESS,
  GET_DATA,
  GET_DATA_SUCCESS,
} from './constants';

export const initialState = fromJS({
  loading: false,
  error: null,
  users: null,
  foundedUsers: null,
  userAdded: false,
  userOnlineData: {
    data: null,
    loading: false,
    error: null,
  },
});

function homePageReducer(state = initialState, action) {
  switch (action.type) {
    case GET_USERS:
      return state.set('loading', true).set('error', null);
    case SET_USERS:
      return state
        .set('loading', false)
        .set('error', null)
        .set('users', fromJS(action.users));
    case GET_USERS_ERROR:
      return state.set('loading', false).set('error', action.error);

    case FIND_USERS_SUCCESS:
      return state.set('foundedUsers', fromJS(action.data.users));
    case FIND_USERS_ERROR:
      return state.set('foundedUsers', fromJS([]));

    case ADD_USER_SUCCESS:
      return state.set('userAdded', true);

    case GET_DATA:
      return state
        .setIn(['userOnlineData', 'loading'], true)
        .setIn(['userOnlineData', 'error'], null);
    case GET_DATA_SUCCESS:
      return state
        .setIn(['userOnlineData', 'loading'], false)
        .setIn(['userOnlineData', 'error'], null)
        .setIn(['userOnlineData', 'data'], fromJS(action.data));

    default:
      return state;
  }
}

export default homePageReducer;
