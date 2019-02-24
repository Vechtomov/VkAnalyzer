/**
 * application entry point
 */
// Needed for redux-saga es6 generator support
import '@babel/polyfill';

// Import all the third party stuff
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import Immutable from 'immutable';
import { ConnectedRouter } from 'connected-react-router/immutable';

import FontFaceObserver from 'fontfaceobserver';
import 'sanitize.css/sanitize.css';
import history from './apphistory';
// Import root app
import App from './containers/App';
import '!file-loader?name=[name].[ext]!./images/favicon.ico';
import configureStore from './configureStore';

// import ru from './moment-ru';
// import moment from 'moment';

// moment.updateLocale('ru', ru);

// until better times
import 'theme/semantic.less';

// import 'semantic-ui-css/semantic.min.css';

// Observe loading of Open Sans (to remove open sans, remove the <link> tag in
// the index.html file and this observer)
const openSansObserver = new FontFaceObserver('Open Sans', {});

// When Open Sans is loaded, add a font-family using Open Sans to the body
openSansObserver.load().then(() => {
  document.body.classList.add('fontLoaded');
});

document.body.classList.add('overflow');

// Create redux store with history
const initialState = Immutable.Map();
const store = configureStore(initialState, history);
const MOUNT_NODE = document.getElementById('app');

const render = () => {
  ReactDOM.render(
    <Provider store={store}>
      <ConnectedRouter history={history}>
        <App />
      </ConnectedRouter>
    </Provider>,
    MOUNT_NODE,
  );
};

render();
