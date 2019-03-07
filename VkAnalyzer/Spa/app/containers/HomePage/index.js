/**
 *
 * HomePage
 *
 */

import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Helmet } from 'react-helmet';
import { createStructuredSelector } from 'reselect';
import { compose, bindActionCreators } from 'redux';

import injectSaga from 'utils/injectSaga';
import injectReducer from 'utils/injectReducer';
import { Button, Grid, Dropdown, Input, Loader } from 'semantic-ui-react';
import {
  makeSelectHomePage,
  makeSelectUsers,
  makeSelectFoundedUsers,
  makeSelectUserOnlineData,
} from './selectors';
import { getUsers, findUsers, addUser, getData } from './actions';
import reducer from './reducer';
import saga from './saga';
import {
  MainContainer,
  MainTitle,
  SectionsDivider,
  // OverflowedBlock,
} from './Wrappers';
import Block from '../../components/Block';
import OnlineChart from '../../components/OnlineChart';

class HomePage extends React.Component {
  state = {
    selectedUser: null,
  };

  componentDidMount() {
    const { getUsers } = this.props;
    getUsers();
  }

  selectUser = (e, { value }) => {
    const { users, getData } = this.props;
    const user = users.find(user => user.get('id') === value);
    this.setState({
      selectedUser: user,
    });
    getData(user.get('id'));
  };

  findUsers = (e, data) => {
    const { findUsers } = this.props;
    findUsers(data.value);
  };

  isUserTracked = id => {
    const { users } = this.props;
    const founded = users.find(u => u.get('id') === id);
    return founded;
  };

  render() {
    const {
      homePage: { loading, error },
      users,
      foundedUsers,
      addUser,
      userOnlineData,
    } = this.props;
    const { selectedUser } = this.state;

    const usersOptions =
      users &&
      Array.from(
        users.map(user => ({
          key: user.get('id'),
          value: user.get('id'),
          text: `${user.get('firstName')} ${user.get('lastName')}`,
        })),
      );

    const onlineData = userOnlineData.toJS();

    return (
      <div>
        <Helmet>
          <title>HomePage</title>
          <meta name="description" content="Description of HomePage" />
        </Helmet>
        <MainContainer>
          <MainTitle as="h1">VK ONLINE TRACKER</MainTitle>

          <SectionsDivider horizontal>Статистика</SectionsDivider>

          <Grid>
            <Grid.Column width={8}>
              <Block>
                <Loader active={onlineData.loading} />
                <Block>
                  {onlineData.data && (
                    <OnlineChart data={onlineData.data.onlineInfos} />
                  )}
                </Block>
              </Block>
            </Grid.Column>
            <Grid.Column width={8}>
              <Block>
                <Dropdown
                  selection
                  search
                  fluid
                  loading={loading}
                  placeholder="Введите имя"
                  options={usersOptions}
                  onChange={this.selectUser}
                  selectOnBlur={false}
                />
              </Block>
              {selectedUser && (
                <Block>
                  {selectedUser.get('firstName')} {selectedUser.get('lastName')}
                </Block>
              )}
            </Grid.Column>
          </Grid>

          <SectionsDivider horizontal>Добавить пользователя</SectionsDivider>

          <Grid>
            <Grid.Column width={8}>
              <Block>Вставьте ссылку</Block>
              <Block>
                <Input fluid onChange={this.findUsers} />
              </Block>
            </Grid.Column>
            <Grid.Column width={8}>
              {foundedUsers &&
                foundedUsers.map(user => (
                  <Block key={user.get('id')}>
                    <div>
                      <a
                        href={`https://vk.com/id${user.get('id')}`}
                        target="_blank"
                      >
                        {user.get('firstName')} {user.get('lastName')}
                      </a>
                    </div>
                    <img src={user.get('photo')} alt={user.get('firstName')} />
                    {!this.isUserTracked(user.get('id')) && (
                      <div>
                        <Button onClick={() => addUser(user.get('id'))}>
                          Добавить
                        </Button>
                      </div>
                    )}
                  </Block>
                ))}
            </Grid.Column>
          </Grid>

          <div>{error}</div>
        </MainContainer>
      </div>
    );
  }
}

HomePage.propTypes = {
  // selectors
  homePage: PropTypes.object,
  users: PropTypes.object,
  foundedUsers: PropTypes.object,
  userOnlineData: PropTypes.object,

  // actions
  getUsers: PropTypes.func,
  findUsers: PropTypes.func,
  addUser: PropTypes.func,
  getData: PropTypes.func,
};

const mapStateToProps = createStructuredSelector({
  homePage: makeSelectHomePage(),
  users: makeSelectUsers(),
  foundedUsers: makeSelectFoundedUsers(),
  userOnlineData: makeSelectUserOnlineData(),
});

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      getUsers,
      findUsers,
      addUser,
      getData,
    },
    dispatch,
  );
}

const withConnect = connect(
  mapStateToProps,
  mapDispatchToProps,
);

const withReducer = injectReducer({ key: 'homePage', reducer });
const withSaga = injectSaga({ key: 'homePage', saga });

export default compose(
  withReducer,
  withSaga,
  withConnect,
)(HomePage);
