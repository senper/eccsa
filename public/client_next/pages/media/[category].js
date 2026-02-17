import React from 'react';
import {
  Grid,
  Card,
  CardActions,
  CardContent,
  Typography,
  createTheme,
} from '@mui/material';
import { getTextMediaList, textMediaTypes } from '../api/otherMedia';
import FaEye from '@mui/icons-material/RemoveRedEye';
import { useRouter } from 'next/router';
import { getLocalByKey, getPathFromTitle } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';
import styles from '../../styles/News.module.css';
import Link from 'next/link';
import Custom404 from '../404';

export default function TextMedia({ translations, medias }) {
  const location = useRouter();
  const { locale } = location;

  const { category } = location.query;

  let mediaCategoryValue = 0;
  if (category) {
    const selectedCategory = textMediaTypes.find(
      (t) => getPathFromTitle(t.text) == category.toLowerCase()
    );
    if (selectedCategory) {
      mediaCategoryValue = selectedCategory.value;
    }
  }

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
      path: `/media/${category}`,
    },
  ];

  const [textMedias, setTextMedias] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    if (mediaCategoryValue) {
      setLoading(true);
      setTextMedias(medias);
      setLoading(false);
    }
  }, [mediaCategoryValue]);
  const theme = createTheme();
  const space = theme.spacing(2);
  const viewsText = (media) =>
    media.views > 999 ? media.views / 1000 : media.views;
  const displayMedias = textMedias.map((value, index) => {
    return (
      <Grid item xs={12} sm={6} md={3} key={index}>
        <Card
          className={styles.card}
          style={{ paddingTop: space, paddingBottom: space }}
        >
          <CardContent>
            <Typography gutterBottom variant="h6" className={styles.title}>
              {getLocalByVal(value.title, value.titleLocal)}
            </Typography>
            <Typography className="body">
              {getLocalByVal(value.brief, value.briefLocal)}
            </Typography>
          </CardContent>
          <CardActions>
            <Link
              href={`/media/detail?mediaCategory=${category}&mediaId=${value.id}`}
              className="btn btn-outline-success">
              {getLocal('Read More')}
            </Link>
            <span>
              <FaEye
                title={'Views Count ' + viewsText(value)}
                className={styles.eyeIconStyle}
              />
              {viewsText(value)}
            </span>
          </CardActions>
        </Card>
      </Grid>
    );
  });
  function getText() {
    return mediaCategoryValue &&
      textMediaTypes.find((s) => s.value == mediaCategoryValue)
      ? textMediaTypes.find((s) => s.value == mediaCategoryValue).text
      : '';
  }

  if (!category || !mediaCategoryValue) {
    return <Custom404 />;
  }

  return (
    <>
      <main>
        <Grid container spacing={2} className={styles.cardGrid}>
          <Grid item xs={12} md={12}>
            <CommonBreadcrumb links={breadcrumbLinks()} />
          </Grid>
          <Grid item xs={11} md={11}>
            <Grid container spacing={1}>
              {textMedias && textMedias.length > 0 ? (
                displayMedias
              ) : loading ? (
                `${getLocal('Loading')}...`
              ) : (
                <h3>
                  {getLocal('No')} {getLocal(getText())} {getLocal('Available')}
                </h3>
              )}
            </Grid>
          </Grid>
        </Grid>
      </main>
    </>
  );
}
export async function getServerSideProps(context) {
  const { category } = context.query;
  let mediaCategoryValue = 0;
  let medias = [];
  if (category) {
    const selectedCategory = textMediaTypes.find(
      (t) => getPathFromTitle(t.text) == category.toLowerCase()
    );
    if (selectedCategory) {
      mediaCategoryValue = selectedCategory.value;
    }
  }

  await getTextMediaList(mediaCategoryValue).then((data) => {
    medias = data.data.$values;
  });

  return {
    props: {
      medias,
    },
  };
}
