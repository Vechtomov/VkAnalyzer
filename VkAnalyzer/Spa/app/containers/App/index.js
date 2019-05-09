import React from 'react';
import { Helmet } from 'react-helmet';
import { Switch, Route } from 'react-router-dom';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { createStructuredSelector } from 'reselect';
import { compose, bindActionCreators } from 'redux';
import { Icon, Transition } from 'semantic-ui-react';

import reducer from './reducer';
import injectReducer from '../../utils/injectReducer';

import HomePage from '../HomePage/Loadable';

import GlobalStyle from '../../global-styles';
import {
  AppWrapper,
  MainWrapper,
  ErrorWrapper,
  CloseIconWrapper,
} from './Wrappers';
import { makeSelectError } from './selectors';
import { setError, clearError } from './actions';

class App extends React.Component {
  render() {
    const { error, clearError } = this.props;
    return (
      <>
        <AppWrapper>
          <Helmet
            titleTemplate="Vk Online Tracker"
            defaultTitle="Vk Online Tracker"
          >
            <meta name="description" content="Vk Online Tracker" />
          </Helmet>
          <MainWrapper>
            <Switch>
              <Route exact path="/" component={HomePage} />
            </Switch>
          </MainWrapper>
          <GlobalStyle />
        </AppWrapper>
        <Transition.Group animation="fade up" duration={300}>
          {error && (
            <ErrorWrapper>
              {error}
              <CloseIconWrapper onClick={clearError}>
                <Icon name="close" />
              </CloseIconWrapper>
            </ErrorWrapper>
          )}
        </Transition.Group>
      </>
    );
  }
}

App.propTypes = {
  // selectors
  error: PropTypes.string,

  // actions
  clearError: PropTypes.func,
};

const mapStateToProps = createStructuredSelector({
  error: makeSelectError(),
});

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      setError,
      clearError,
    },
    dispatch,
  );
}

const withConnect = connect(
  mapStateToProps,
  mapDispatchToProps,
);

const withReducer = injectReducer({ key: 'app', reducer });

export default compose(
  withReducer,
  withConnect,
)(App);
