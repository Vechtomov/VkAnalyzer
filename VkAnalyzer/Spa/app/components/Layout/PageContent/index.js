import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

const PageContentWrapper = styled.div`
  max-width: 1600px;
  margin: 0 auto;
  padding: 0 1rem;
`;

const PageContent = props => (
  <PageContentWrapper>{props.children}</PageContentWrapper>
);

PageContent.propTypes = {
  children: PropTypes.any.isRequired,
};

export default PageContent;
