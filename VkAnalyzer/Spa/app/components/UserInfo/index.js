import React from 'react';
import PropTypes from 'prop-types';

const UserInfo = ({ user }) => (
  <div>
    <a href={`https://vk.com/id${user.get('id')}`} target="_blank">
      {user.get('firstName')} {user.get('lastName')}
    </a>
    <div>
      <img src={user.get('photo')} alt={user.get('firstName')} />
    </div>
  </div>
);

UserInfo.propTypes = {
  user: PropTypes.object,
};

export default UserInfo;
