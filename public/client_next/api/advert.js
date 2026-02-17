import axios from 'axios';

const config = {
  clientId: 'react.client',
  adImagesUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiAdvert/GetAdImages`,
  getSettingValueUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiAdvert/GetSettingValue`,
  getSettingValuesUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiAdvert/GetSettingValues`,

  scopes: [],
};
//home_slider_width
export const getSettingValue = (key) => {
  return axios.get(config.getSettingValueUrl + '?key=' + key);
};
export const getSettingValues = (listKey) => {
  let keys = '';

  for (let i = 0; i < listKey.length; i++) {
    if (i === 0) {
      keys = '?keys=' + listKey[i];
    } else {
      keys += '&keys=' + listKey[i];
    }
  }

  return axios.get(config.getSettingValuesUrl + keys);
};
export const getAdImages = (type) => {
  return axios.get(config.adImagesUri + '?section=' + type);
};
