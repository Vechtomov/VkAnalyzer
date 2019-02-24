/**
 *
 * App
 *
 * This component is the skeleton around the actual pages, and should only
 * contain code that should be seen on all pages. (e.g. navigation bar)
 */

import React from 'react';
import { Helmet } from 'react-helmet';
import styled from 'styled-components';
import { Switch, Route } from 'react-router-dom';

import HomePage from '../HomePage/Loadable';
import Test from '../Test/Loadable';

import GlobalStyle from '../../global-styles';

const AppWrapper = styled.div`
  display: flex;
  min-height: 100vh;
  position: relative;
  flex-direction: column;
`;

const MainWrapper = styled.div`
  flex: 1 0 auto;
  padding: 0;
  min-height: calc(100vh - 78px);
`;

export default function App() {
  return (
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
          <Route exact path="/test" component={Test} />
        </Switch>
      </MainWrapper>
      <GlobalStyle />
    </AppWrapper>
  );
}
