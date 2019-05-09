import { createSelector } from 'reselect';
import { initialState } from './reducer';

const selectHomePageDomain = state => state.get('homePage', initialState);

const makeSelectHomePage = () =>
  createSelector(selectHomePageDomain, substate => substate.toJS());

const makeSelectUsers = () =>
  createSelector(selectHomePageDomain, substate => {
    const users = substate.get('users');
    if (!users) return null;
    let userName = substate.get('userName');
    if (!userName) return users.slice(0, 10);

    userName = userName.toLowerCase();

    return users
      .filter(u =>
        `${u.get('firstName').toLowerCase()} ${u
          .get('lastName')
          .toLowerCase()}`.includes(userName),
      )
      .slice(0, 10);
  });

const makeSelectAllUsers = () =>
  createSelector(selectHomePageDomain, substate => substate.get('users'));

const makeSelectUsersCount = () =>
  createSelector(selectHomePageDomain, substate =>
    substate.getIn(['stat', 'usersCount']),
  );

const makeSelectFoundedUsers = () =>
  createSelector(selectHomePageDomain, substate =>
    substate.get('foundedUsers'),
  );

const makeSelectUserOnlineData = () =>
  createSelector(selectHomePageDomain, substate =>
    substate.get('userOnlineData'),
  );

const makeSelectUserName = () =>
  createSelector(selectHomePageDomain, substate => substate.get('userName'));

const makeSelectUser = () =>
  createSelector(selectHomePageDomain, substate => substate.get('user'));

export {
  makeSelectHomePage,
  makeSelectUsers,
  makeSelectUsersCount,
  makeSelectFoundedUsers,
  makeSelectUserOnlineData,
  makeSelectUserName,
  makeSelectAllUsers,
  makeSelectUser,
};
