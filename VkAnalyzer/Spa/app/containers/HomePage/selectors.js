import { createSelector } from 'reselect';
import { initialState } from './reducer';

/**
 * Direct selector to the homePage state domain
 */

const selectHomePageDomain = state => state.get('homePage', initialState);

/**
 * Other specific selectors
 */

/**
 * Default selector used by HomePage
 */

const makeSelectHomePage = () =>
  createSelector(selectHomePageDomain, substate => substate.toJS());

const makeSelectUsers = () =>
  createSelector(selectHomePageDomain, substate => substate.get('users'));

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
  makeSelectFoundedUsers,
  makeSelectUserOnlineData,
};
