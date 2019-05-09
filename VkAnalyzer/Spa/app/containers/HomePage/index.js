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

// import Calendar from 'react-calendar';
import DatePicker, { registerLocale } from 'react-datepicker';

import 'react-datepicker/dist/react-datepicker.css';
import ru from 'date-fns/locale/ru';
registerLocale('ru', ru);

import {
  Button,
  Grid,
  Dropdown,
  Input,
  Loader,
  Popup,
} from 'semantic-ui-react';
import injectReducer from '../../utils/injectReducer';
import injectSaga from '../../utils/injectSaga';
import {
  makeSelectHomePage,
  makeSelectUsers,
  makeSelectFoundedUsers,
  makeSelectUserOnlineData,
  makeSelectUsersCount,
  makeSelectUserName,
  makeSelectAllUsers,
  makeSelectUser,
} from './selectors';
import {
  getUsers,
  findUsers,
  addUser,
  getData,
  getFriends,
  usernameChanged,
} from './actions';
import reducer from './reducer';
import saga from './saga';
import {
  MainContainer,
  MainTitle,
  SectionsDivider,
  DatePickerWrapper,
  ChooseDateItemWrapper,
  ChartWrapper,
} from './Wrappers';
import Block from '../../components/Block';
import OnlineChart from '../../components/OnlineChart';
import UserInfo from '../../components/UserInfo';

class HomePage extends React.Component {
  state = {
    // selectedUser: null,
    from: null,
    to: null,
  };

  componentDidMount() {
    const { getUsers } = this.props;
    getUsers();
  }

  selectUser = (e, { value }) => {
    const { users, getData } = this.props;
    const user = users.find(user => user.get('id') === value);
    // this.setState({
    //   selectedUser: user,
    // });
    getData(user.get('id'));
  };

  onUsersDropdownSearchChange = (e, data) => {
    const { usernameChanged } = this.props;
    usernameChanged(data.searchQuery);
  };

  findUsers = (e, data) => {
    const { findUsers } = this.props;
    findUsers(data.value);
  };

  isUserTracked = id => {
    const { allUsers } = this.props;
    const founded = allUsers.find(u => u.get('id') === id);
    return founded;
  };

  render() {
    const {
      homePage: { loading, error },
      users,
      usersCount,
      foundedUsers,
      addUser,
      userOnlineData,
      getData,
      // getFriends,
      userName,
      user,
    } = this.props;
    const { from, to } = this.state;

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
          <MainTitle as="h1">Статистика посещаемости ВКонтакте</MainTitle>

          <SectionsDivider horizontal>Статистика</SectionsDivider>
          <div style={{ marginBottom: '20px' }}>
            Количество пользователей: {usersCount}
            <Popup
              trigger={
                <a style={{ marginLeft: '25px' }} href="/">
                  Посмотреть общую статистику
                </a>
              }
              content="Нереализованный функционал"
            />
          </div>

          <Grid>
            <Grid.Column computer={8} mobile={16}>
              <Dropdown
                selection
                search
                fluid
                value={userName}
                loading={loading}
                placeholder="Введите имя"
                options={usersOptions}
                onChange={this.selectUser}
                selectOnBlur={false}
                onSearchChange={this.onUsersDropdownSearchChange}
              />
            </Grid.Column>
            <Grid.Column computer={8} mobile={16}>
              {user && (
                <Grid>
                  <Grid.Column width={5}>
                    <UserInfo user={user} />
                  </Grid.Column>
                  <Grid.Column width={11}>
                    {user.get('friendsCount') && (
                      <Block>Друзей: {user.get('friendsCount')}</Block>
                    )}
                    <Popup
                      trigger={
                        <Block>Время в сети за сегодня: 1 ч. 15 мин.</Block>
                      }
                      content="Нереализованный функционал"
                    />

                    <Popup
                      trigger={
                        <Block>Время в сети за неделю: 6 ч. 38 мин.</Block>
                      }
                      content="Нереализованный функционал"
                    />

                    {/* <Button
                      onClick={() => getFriends(user.get('id'))}
                      content="Добавить друзей"
                    /> */}
                  </Grid.Column>
                </Grid>
              )}
            </Grid.Column>
          </Grid>
          <div>
            <Loader active={onlineData.loading} />
            {onlineData.data && (
              <div>
                <div>График онлайна:</div>
                <ChartWrapper>
                  <OnlineChart data={onlineData.data.onlineInfos} />
                </ChartWrapper>
                <div>
                  <ChooseDateItemWrapper>
                    <DatePickerWrapper>
                      <DatePicker
                        selected={from}
                        onChange={date => this.setState({ from: date })}
                        dateFormat="dd.MM.YYYY HH:mm"
                        locale="ru"
                        placeholderText="От"
                        showTimeSelect
                        timeFormat="HH:mm"
                        timeIntervals={15}
                        timeCaption="Время"
                        isClearable
                      />
                    </DatePickerWrapper>
                  </ChooseDateItemWrapper>

                  <ChooseDateItemWrapper>
                    <DatePickerWrapper>
                      <DatePicker
                        selected={to}
                        onChange={date => this.setState({ to: date })}
                        dateFormat="dd.MM.YYYY HH:mm"
                        locale="ru"
                        placeholderText="До"
                        showTimeSelect
                        timeFormat="HH:mm"
                        timeIntervals={15}
                        timeCaption="Время"
                        isClearable
                      />
                    </DatePickerWrapper>
                  </ChooseDateItemWrapper>

                  <ChooseDateItemWrapper>
                    <Button
                      onClick={() =>
                        getData(
                          user.get('id'),
                          from && Math.round(from),
                          to && Math.round(to),
                        )
                      }
                      size="medium"
                    >
                      Обновить
                    </Button>
                  </ChooseDateItemWrapper>
                </div>
              </div>
            )}
          </div>

          <SectionsDivider horizontal>Добавить пользователя</SectionsDivider>

          <Grid>
            <Grid.Column computer={8} tablet={16}>
              <div>Вставьте ссылку</div>
              <div>
                <Input fluid onChange={this.findUsers} />
              </div>
            </Grid.Column>
            <Grid.Column computer={8} tablet={16}>
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
                      <div style={{ marginTop: '10px' }}>
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
  usersCount: PropTypes.number,
  foundedUsers: PropTypes.object,
  userOnlineData: PropTypes.object,
  userName: PropTypes.string,
  allUsers: PropTypes.object,
  user: PropTypes.object,

  // actions
  getUsers: PropTypes.func,
  findUsers: PropTypes.func,
  addUser: PropTypes.func,
  getData: PropTypes.func,
  // getFriends: PropTypes.func,
  usernameChanged: PropTypes.func,
};

const mapStateToProps = createStructuredSelector({
  homePage: makeSelectHomePage(),
  users: makeSelectUsers(),
  usersCount: makeSelectUsersCount(),
  foundedUsers: makeSelectFoundedUsers(),
  userOnlineData: makeSelectUserOnlineData(),
  userName: makeSelectUserName(),
  allUsers: makeSelectAllUsers(),
  user: makeSelectUser(),
});

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      getUsers,
      findUsers,
      addUser,
      getData,
      getFriends,
      usernameChanged,
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
