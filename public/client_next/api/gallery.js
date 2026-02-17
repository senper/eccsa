import axios from 'axios';

const config = {
  clientId: 'react.client',
  photoAlbumsUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiGallery/PhotoAlbums`,
  getAlbumPhotosUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiGallery/GetAlbumPhotos`,
  videoGalleriesUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiGallery/VideoGalleries`,
  scopes: [],
};

export const getPhotoAlbums = () => {
  return axios.get(config.photoAlbumsUri);
};

export const getAlbumPhotos = (id) => {
  return axios.get(config.getAlbumPhotosUri + '/' + id);
};

export const getVideoGalleries = () => {
  return axios.get(config.videoGalleriesUri);
};

export const downloadLaw = (id) => {
  return axios.get(config.downloadUri + '/' + id);
};
