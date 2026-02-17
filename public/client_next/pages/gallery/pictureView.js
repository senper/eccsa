import React from 'react';
import { Grid } from '@mui/material';
import ImageGallery from 'react-image-gallery';
import 'react-image-gallery/styles/css/image-gallery.css';
import { getAlbumPhotos } from '../../api/gallery';
import { Row } from 'react-bootstrap';
import { useRouter } from 'next/router';
import { getLocalByKey } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';

export default function PictureView({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  let albumIdValue = 0;
  if (location.query) {
    const { albumId } = location.query;
    albumIdValue = albumId;
  }
  const [photos, setPhotos] = React.useState([]);
  React.useEffect(() => {
    if (albumIdValue) {
      getAlbumPhotos(albumIdValue).then((data) => {
        setPhotos(data.data.$values);
      });
    }
  }, [albumIdValue]);

  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Albums'),
      path: '/gallery/photoGallery',
    },
    {
      title: getLocal('Photos'),
      path: '#',
    },
  ];
  return (
    <>
      <div>
        <Grid container spacing={2} className="grid">
          <div className="paddedMainDiv">
            <CommonBreadcrumb links={breadcrumbLinks()} />
          </div>
          <Grid item xs={12} md={12}>
            <Row>
              <ImageGallery
                items={photos.map((v) => ({
                  original: process.env.NEXT_PUBLIC_API_URL + '/' + v,
                  thumbnail: process.env.NEXT_PUBLIC_API_URL + '/' + v,
                }))}
              />
            </Row>
          </Grid>
        </Grid>
      </div>
    </>
  );
}
