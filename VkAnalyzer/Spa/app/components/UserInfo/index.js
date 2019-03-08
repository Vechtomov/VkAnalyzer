import React from 'react';
import PropTypes from 'prop-types';
import Block from '../Block';

const UserInfo = ({ user }) => (
  <Block>
    <a href={`https://vk.com/id${user.get('id')}`} target="_blank">
      {user.get('firstName')} {user.get('lastName')}
    </a>
    <div>
      <img src={user.get('photo')} alt={user.get('firstName')} />
    </div>
  </Block>
);

UserInfo.propTypes = {
  user: PropTypes.object,
};

export default UserInfo;
