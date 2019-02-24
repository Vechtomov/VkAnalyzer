import { request } from '../../utils/request';
import URL from './constants';

export default class UsersService {
  static getUsers = () => {
    const options = {
      method: 'GET',
    };

    return request(URL.GET_USERS, options);
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

  static addUser = ids => {
    const options = {
      method: 'POST',
      body: {
        ids,
      },
    };

    return request(URL.ADD_USER, options);
  };
}
