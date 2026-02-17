import React from 'react';

import { Grid } from '@mui/material';
import 'react-image-gallery/styles/css/image-gallery.css';
import YouTube from 'react-youtube-embed';
import { Card } from 'react-bootstrap';
import { getVideoGalleries } from '../../api/gallery';
import { useRouter } from 'next/router';
import { getLocalByKey } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';

export default function VideoGallery({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  const [loading, setLoading] = React.useState(false);
  const [videos, setVideos] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getVideoGalleries()
      .then((data) => {
        setVideos(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }, []);

  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Videos'),
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
            <div className="row" style={{ padding: '1rem' }}>
              {loading ? <h3>{getLocal('loading')}...</h3> : null}
              {videos
                ? videos.map((row, index) => (
                    <div
                      className="col-sm-4"
                      style={{ marginBottom: '10px' }}
                      key={index}
                    >
                      <YouTube id={row.url} />
                      <Card>
                        <Card.Title>{row.title}</Card.Title>
                        <Card.Text>{row.description}</Card.Text>
                      </Card>
                    </div>
                  ))
                : null}
              {!loading && (!videos || videos.length <= 0) ? (
                <h3>{getLocal('NOTING FOUND')}</h3>
              ) : null}
            </div>
          </Grid>
        </Grid>
      </div>
    </>
  );
}
