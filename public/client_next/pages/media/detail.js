import React from 'react';
import { Grid, CardContent } from '@mui/material';
import { addComment, getTextMedia, textMediaTypes } from '../api/otherMedia';
import { Card, Col, Row } from 'react-bootstrap';
import FaEye from '@mui/icons-material/RemoveRedEye';
import { useRouter } from 'next/router';
import { getLocalByKey, getPathFromTitle } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';
import styles from '../../styles/News.module.css';

export default function TextMediaDetail({ translations, serverMedia }) {
  var token = localStorage.getItem('access_token');
  const [media, setMedia] = React.useState(null);
  const location = useRouter();
  const { locale } = location;

  let mediaCategoryValue = 0;
  if (location.query && location.query.mediaCategory) {
    const { mediaCategory } = location.query;
    mediaCategoryValue = mediaCategory;
  }
  let mediaIdValue = null;
  if (location.query && location.query.mediaId) {
    const { mediaId } = location.query;
    mediaIdValue = mediaId;
  }
  React.useEffect(() => {
    if (mediaIdValue) {
      setMedia(serverMedia);
    }
  }, [mediaIdValue]);
  function loadMedia() {
    getTextMedia(mediaIdValue).then((data) => {
      setMedia(data.data);
    });
  }
  function getText() {
    return mediaCategoryValue &&
      textMediaTypes.find(
        (s) => getPathFromTitle(s.text) == mediaCategoryValue.toLowerCase()
      )
      ? textMediaTypes.find(
          (s) => getPathFromTitle(s.text) == mediaCategoryValue.toLowerCase()
        ).text
      : '';
  }

  const [comment, setComment] = React.useState(null);
  function saveComment(event) {
    event.preventDefault();
    addComment(mediaIdValue, comment).then((data) => {
      setComment('');
      loadMedia();
    });
  }

  const formateProfile = () => {
    let profile = getLocalByVal(media.content, media.contentLocal);
    profile = profile.replaceAll(
      'src="/pr/text_media',
      `src="${process.env.NEXT_PUBLIC_API_URL}/pr/text_media`
    );
    return profile;
  };
  const viewsText = media
    ? media.views > 999
      ? media.views / 1000
      : media.views
    : null;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };

  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal(getText()),
      path: `/media/${mediaCategoryValue}`,
    },
    {
      title: getLocal('Details'),
      path: '#',
    },
  ];
  return (
    <main>
      <Grid container spacing={2} className={styles.cardGrid}>
        <Grid item xs={12} md={12}>
          <CommonBreadcrumb links={breadcrumbLinks()} />
        </Grid>
        <Grid item xs={11} md={11}>
          {media ? (
            <Card>
              <Card.Header className={styles.cardHeader2}>
                {getLocalByVal(media.title, media.titleLocal)}
              </Card.Header>
              <CardContent>
                <>
                  <p dangerouslySetInnerHTML={{ __html: formateProfile() }}></p>
                  <h6>
                    {getLocal('Author')}:
                    {getLocalByVal(media.authorName, media.authorNameLocal)}
                  </h6>
                  <br></br>
                  <h6>{media.dateCreatedStr}</h6>
                </>
                <hr
                  style={{
                    borderWidth: '10px',
                    width: '100%',
                    margin: '5px 0 5px 0',
                  }}
                />
                <Row>
                  <Col>
                    <FaEye
                      title={'Views Count ' + viewsText}
                      className={styles.eyeIconStyle}
                    />
                    {viewsText}
                  </Col>
                </Row>
              </CardContent>
              <Card.Footer>
                <Row>
                  <h3>{getLocal('Comments')}</h3>
                  {media.comments &&
                  media.comments.$values &&
                  media.comments.$values.length > 0 ? (
                    media.comments.$values.map((r, index) => (
                      <Card style={{ marginBottom: '10px' }} key={index}>
                        <Card.Body className="card-subtitle mb-2 text-muted">
                          {r.text}
                        </Card.Body>
                        <div style={{ fontStyle: 'italic' }} role="alert">
                          <p>
                            <small>
                              {getLocal('Written by')}:
                              {getLocalByVal(r.authorName, r.authorNameLocal)}
                            </small>
                          </p>
                        </div>
                      </Card>
                    ))
                  ) : (
                    <strong>{getLocal('No Comments yet')}</strong>
                  )}
                </Row>
                {token ? (
                  <>
                    <h3>{getLocal('Write your Comments')}</h3>
                    <form
                      onSubmit={saveComment}
                      className="comment-form form-group"
                    >
                      <div className="form-group">
                        <label htmlFor="exampleFormControlTextarea1">
                          {getLocal('Comments')}
                        </label>
                        <textarea
                          className="form-control"
                          rows="3"
                          value={comment == null ? '' : comment}
                          onChange={(evt) => setComment(evt.target.value)}
                        ></textarea>
                      </div>
                      <input
                        style={{ margin: '8px 0 0 0 ' }}
                        type="submit"
                        value="Save"
                        className="btn btn-primary"
                      />
                    </form>
                  </>
                ) : null}
              </Card.Footer>
            </Card>
          ) : null}
        </Grid>
      </Grid>
    </main>
  );
}

export async function getServerSideProps(context) {
  const { locale } = context;
  let serverMedia = null;

  let mediaIdValue = null;
  if (context.query && context.query.mediaId) {
    const { mediaId } = context.query;
    mediaIdValue = mediaId;
  }

  await getTextMedia(mediaIdValue).then((data) => {
    serverMedia = data.data;
  });

  return {
    props: {
      serverMedia,
    },
  };
}
