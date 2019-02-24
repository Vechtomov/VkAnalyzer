import styled from 'styled-components';
import { Container, Header, Divider } from 'semantic-ui-react';

export const MainContainer = styled(Container)`
  padding: 3rem;
`;

export const MainTitle = styled(Header)`
  text-align: center;
  margin-bottom: 2em;
`;

export const SectionsDivider = styled(Divider)`
  &&& {
    font-size: 18px;
  }
`;
