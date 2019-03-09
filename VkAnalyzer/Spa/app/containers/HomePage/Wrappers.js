import styled from 'styled-components';
import { Container, Header, Divider } from 'semantic-ui-react';
import Block from '../../components/Block';

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
    margin: 3em 0 2em;
  }
`;

export const OverflowedBlock = styled(Block)`
  overflow: auto;
`;

export const DatePickerWrapper = styled.div`
  input {
    margin: 0;
    max-width: 100%;
    -webkit-box-flex: 1;
    -ms-flex: 1 0 auto;
    flex: 1 0 auto;
    outline: 0;
    -webkit-tap-highlight-color: rgba(255, 255, 255, 0);
    text-align: left;
    line-height: 1.21428571em;
    font-family: Lato, 'Helvetica Neue', Arial, Helvetica, sans-serif;
    padding: 0.67857143em 1em;
    background: #fff;
    border: 1px solid rgba(34, 36, 38, 0.15);
    color: rgba(0, 0, 0, 0.87);
    border-radius: 0.28571429rem;
    -webkit-transition: border-color 0.1s ease, -webkit-box-shadow 0.1s ease;
    transition: border-color 0.1s ease, -webkit-box-shadow 0.1s ease;
    transition: box-shadow 0.1s ease, border-color 0.1s ease;
    transition: box-shadow 0.1s ease, border-color 0.1s ease,
      -webkit-box-shadow 0.1s ease;
    -webkit-box-shadow: none;
    box-shadow: none;
  }
  input:focus {
    border-color: #85b7d9;
    background: #fff;
    color: rgba(0, 0, 0, 0.8);
    -webkit-box-shadow: none;
    box-shadow: none;
  }
`;

export const ChooseDateWrapper = styled(Block)`
  display: flex;
  flex-direction: row;
  align-items: center;
`;

export const ChooseDateItemWrapper = styled.div`
  display: inline-block;
  margin-right: 1em;
  margin-top: 1em;
`;

export const ChartWrapper = styled.div`
  margin: 2em 0;
`;
