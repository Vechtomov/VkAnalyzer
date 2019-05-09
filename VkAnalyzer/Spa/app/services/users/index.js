import { request } from '../../utils/request';
import URL from './constants';

export default class UsersService {
  static getUsers = () => {
    const options = {
      method: 'GET',
    };

    return request(URL.GET_USERS, options);
  };

  static getUsersCount = () => {
    const options = {
      method: 'GET',
    };

    return request(URL.GET_USERS_COUNT, options);
  };

  static findUsers = filter => {
    const options = {
      method: 'GET',
      params: {
        filter,
      },
    };

    return request(URL.FIND_USERS, options);
  };

  static addUsers = ids => {
    const options = {
      method: 'POST',
      body: {
        ids,
      },
    };

    return request(URL.ADD_USER, options);
  };

  static getData = (id, from, to) => {
    const options = {
      method: 'GET',
      params: {
        id,
        from,
        to,
      },
    };

    return request(URL.GET_DATA, options);
  };

  static getFriends = userId => {
    const options = {
      method: 'GET',
      params: {
        userId,
      },
    };

    return request(URL.GET_FRIENDS, options);
  };

  static getUserInfo = id => {
    const options = {
      method: 'GET',
      params: {
        id,
      },
    };

    return request(URL.GET_USER_INFO, options);
  };
}
