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
  SET_USERS_COUNT,
  USERNAME_CHANGED,
  SET_USER_INFO,
} from './constants';

const mapOnlineData = infos => {
  if (!infos || infos.length === 0) return infos;

  let startTime = null;
  let endTime = null;
  let ranges = [];

  // заносим в массив все отрезки
  for (let i = 0; i < infos.length - 1; i += 1) {
    startTime = infos[i].date;
    endTime = infos[i + 1].date;

    ranges.push({
      startTime,
      endTime,
      type: infos[i].onlineInfo,
    });
  }

  startTime = new Date(endTime);

  ranges.push({
    startTime,
    endTime: new Date(),
    type: infos[infos.length - 1].onlineInfo,
  });

  // удаляем оффлайн отрезки
  ranges = ranges.filter(range => range.type !== 1);

  return ranges;
};

export const initialState = fromJS({
  loading: false,
  error: null,
  users: null,
  userName: undefined,
  user: null,
  stat: {
    usersCount: 0,
  },
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

    case SET_USERS_COUNT:
      return state.setIn(['stat', 'usersCount'], action.count);

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
        .setIn(
          ['userOnlineData', 'data'],
          fromJS({
            ...action.data,
            onlineInfos: mapOnlineData(action.data.onlineInfos),
          }),
        );

    case USERNAME_CHANGED:
      return state.set('userName', action.name);

    case SET_USER_INFO:
      return state.set('user', fromJS(action.info));

    default:
      return state;
  }
}

export default homePageReducer;
