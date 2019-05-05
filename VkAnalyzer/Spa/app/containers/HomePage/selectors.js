import { createSelector } from 'reselect';
import { initialState } from './reducer';

const selectHomePageDomain = state => state.get('homePage', initialState);

const makeSelectHomePage = () =>
  createSelector(selectHomePageDomain, substate => substate.toJS());

const makeSelectUsers = () =>
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

export {
  makeSelectHomePage,
  makeSelectUsers,
  makeSelectUsersCount,
  makeSelectFoundedUsers,
  makeSelectUserOnlineData,
};
