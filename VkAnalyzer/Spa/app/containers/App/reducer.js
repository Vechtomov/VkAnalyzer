import { fromJS } from 'immutable';

import { SET_ERROR, CLEAR_ERROR } from './constants';

const initialState = fromJS({
  error: null,
});

function appReducer(state = initialState, action) {
  switch (action.type) {
    case SET_ERROR:
      return state.set('error', action.error);
    case CLEAR_ERROR:
      return state.set('error', null);
    default:
      return state;
  }
}

export default appReducer;
