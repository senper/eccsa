import React from 'react';
import { Grid } from '@mui/material';
import { Card } from 'react-bootstrap';
import { getPhotoAlbums } from '../../api/gallery';
import { useRouter } from 'next/router';
import { getLocalByKey } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';
import Link from 'next/link';

export default function PhotoGallery({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  const [loading, setLoading] = React.useState(false);
  const [albums, setAlbums] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getPhotoAlbums()
      .then((data) => {
        setAlbums(data.data.$values);
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
      title: getLocal('Albums'),
      path: '#',
    },
  ];
  return <>
    <Grid>
      <Grid container spacing={2} className="grid">
        <div className="paddedMainDiv">
          <CommonBreadcrumb links={breadcrumbLinks()} />
        </div>
        <Grid item xs={12} md={12}>
          <div className="row" style={{ padding: '1rem' }}>
            {loading ? <h3>{getLocal('loading')}...</h3> : null}
            {albums
              ? albums.map((row, index) => (
                  <div
                    className="col-sm-3"
                    style={{ marginBottom: '10px' }}
                    key={index}
                  >
                    <Card>
                      <Card.Img
                        variant="top"
                        src={row.imageurl}
                        style={{ width: '20px' }}
                      />
                      <Card.Body>
                        <Card.Title>{row.title}</Card.Title>
                        <Card.Text>{row.description}</Card.Text>
                        <Link
                          href={{
                            pathname: '/gallery/pictureView',
                            query: {
                              albumId: row.id,
                            },
                          }}
                          legacyBehavior>
                          {getLocal('View')}
                        </Link>
                      </Card.Body>
                    </Card>
                  </div>
                ))
              : null}
            {!loading && (!albums || albums.length <= 0) ? (
              <h3>{getLocal('NOTING FOUND')}</h3>
            ) : null}
          </div>
        </Grid>
      </Grid>
    </Grid>
  </>;
}
