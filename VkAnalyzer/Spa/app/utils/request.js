import AppSettings from '../appSettings';
import auth from './auth';

/**
 * Parses the JSON returned by a network request
 *
 * @param  {object} response A response from a network request
 *
 * @return {object}          The parsed JSON from the request
 */
function parseJSON(response) {
  if (response.status === 204 || response.status === 205) {
    return null;
  }
  return response.json();
}

/**
 * Checks if a network request came back fine, and throws an error if not
 *
 * @param  {object} response   A response from a network request
 *
 * @return {object|undefined} Returns either the response, or throws an error
 */
export function checkStatus(response) {
  if (response.status >= 200 && response.status < 300) {
    return response;
  }

  if (response.status === 401) {
    auth.clearToken();
    window.location = '/';
  }

  if (!response.ok) {
    const err = new Error(response.statusText);
    err.response = response;
    throw err;
  }

  return response.json().then(res => {
    if (res) {
      throw res;
    } else {
      const error = new Error(response.statusText);
      error.response = response;
      throw error;
    }
  });
}

/**
 * Format query params
 *
 * @param params
 * @returns {string}
 */
export function formatQueryParams(params) {
  function iter(o, path) {
    if (Array.isArray(o)) {
      o.forEach(a => {
        iter(a, `${path}`);
      });
      return;
    }
    if (o !== null && typeof o === 'object') {
      Object.keys(o).forEach(k => {
        iter(o[k], `${path}.${k}`);
      });
      return;
    }
    if (typeof o === 'boolean' || o || o === 0) {
      data.push(`${path}=${encodeURIComponent(o)}`);
    }
  }

  let data = [];
  Object.keys(params).forEach(k => {
    iter(params[k], k);
  });
  return data.join('&');
}

function formatInlineParam(inlineParam) {
  return inlineParam[Object.keys(inlineParam)[0]];
}
/**
 * Requests a URL, returning a promise
 *
 * @param  {string} url       The URL we want to request
 * @param  {object} [options] The options we want to pass to "fetch"
 *
 * @return {object}           The response data
 */
/* eslint no-param-reassign: 0 */
export function request(url, options, baseUrl = AppSettings.webApiUrl) {
  const token = auth.getToken();

  options.credentials = 'include';
  options.headers = Object.assign(
    {
      'Content-Type': 'application/json',
      Token: `${token}`,
    },
    options.headers,
    {},
  );

  if (options && options.inlineParam) {
    const inlineParam = formatInlineParam(options.inlineParam);
    url = `${url}/${inlineParam}`;
  }

  if (options && options.params) {
    const params = formatQueryParams(options.params);
    url = `${url}?${params}`;
  }

  // Stringify body object
  if (options && options.body) {
    options.body = JSON.stringify(options.body);
  }

  return fetch(baseUrl + url, options)
    .then(checkStatus)
    .then(parseJSON)
    .catch(error => {
      throw error;
    });
}

export function multipartRequest(
  url,
  options,
  baseUrl = AppSettings.webApiUrl,
) {
  const token = auth.getToken();

  options.credentials = 'include';
  options.headers = Object.assign(
    {
      Token: `${token}`,
    },
    options.headers,
    {},
  );

  return fetch(baseUrl + url, options)
    .then(checkStatus)
    .then(parseJSON)
    .catch(error => {
      throw error;
    });
}
