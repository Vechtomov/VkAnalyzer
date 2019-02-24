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
} from './constants';

export const initialState = fromJS({
  loading: false,
  error: null,
  users: null,
  foundedUsers: null,
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

    default:
      return state;
  }
}

export default homePageReducer;
