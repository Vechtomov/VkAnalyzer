import styled from 'styled-components';

export const AppWrapper = styled.div`
  display: flex;
  min-height: 100vh;
  position: relative;
  flex-direction: column;
`;

export const MainWrapper = styled.div`
  flex: 1 0 auto;
  padding: 0;
  min-height: calc(100vh - 78px);
`;

export const ErrorWrapper = styled.div`
  position: fixed;
  width: 100%;
  bottom: 0;
  padding: 1em;
  text-align: center;
  background: red;
  color: white;
  margin-left: calc(100% - 100vw);
  z-index: 1000;
`;

export const CloseIconWrapper = styled.div`
  position: absolute;
  right: 10px;
  top: 20%;
  padding: 5px;
  cursor: pointer;
`;
