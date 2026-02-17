import {
  Card,
  CardActions,
  CardContent,
  CardMedia,
  Container,
  Grid,
  Typography,
} from '@mui/material';
import Link from 'next/link';
import { useRouter } from 'next/router';
import React from 'react';
import { getNews } from '../../api/news';
import styles from '../../styles/News.module.css';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';
import { getLocalByKey, getLocalForVal, getTranslations } from '../api/menu';

export default function News({ news, translations, keyWords, setAdditionalKeywords, setPageTitle }) {
  const router = useRouter();
  const { locale } = router;
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
      title: getLocal('News'),
      path: '#',
    },
  ];

  React.useEffect(() => {
    setAdditionalKeywords(keyWords);
    setPageTitle("News List");
  }, [keyWords]);

  const [loading, setLoading] = React.useState(false);

  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace('-', '');
    return `${process.env.NEXT_PUBLIC_API_URL}/pr/documents/${name}${extension}`;
  };
  const displayNews = news.map((newsValue, index) => {
    return (
      <Grid item xs={12} sm={6} md={3} key={index}>
        <Card className={styles.card}>
          {newsValue && newsValue.bannerSetted ? (
            <CardMedia
              className={styles.cardMedia}
              image={imgurl(newsValue.id, newsValue.extension)}
              title={getLocalByVal(newsValue.title, newsValue.titleLocal)}
            />
          ) : null}
          <CardContent className={styles.cardContent}>
            <Typography gutterBottom variant="h6" className={styles.title}>
              {getLocalByVal(newsValue.title, newsValue.titleLocal)}
            </Typography>
            <Typography className="body">
              {getLocalByVal(newsValue.brief, newsValue.briefLocal)}
            </Typography>
          </CardContent>
          <CardActions>
            <Link
              href={`/news/detail?news_id=${newsValue.id}`}
              className="btn btn-outline-success"
              style={{ margin: '0' }}>

              {getLocal('Read More')}

            </Link>
          </CardActions>
        </Card>
      </Grid>
    );
  });

  return (
    <main>
      <Grid container spacing={2} className={styles.cardGrid}>
        <Grid item xs={12} md={12}>
          <CommonBreadcrumb links={breadcrumbLinks()} />
        </Grid>
        <Grid item xs={11} md={11}>
          <Grid container spacing={1}>
            {news && news.length > 0 ? (
              displayNews
            ) : (
              <h3>{getLocal('No News Available')}</h3>
            )}
          </Grid>
        </Grid>
      </Grid>
    </main>
  );
}

export async function getServerSideProps(context) {
  const { locale, res } = context;
  res.setHeader(
    'Cache-Control',
    'public, s-maxage=10, stale-while-revalidate=59'
  );
  let news = [];
  let translations = [];
  let keyWords = [];

  await getTranslations()
    .then((data) => {
      translations = data.data.$values;
    })
    .catch((error) => {});

  await getNews()
    .then((data) => {
      data.data.$values.forEach((s) => {
        const title = getLocalForVal(s.title, s.titleLocal, locale);

        news.push({
          id: s.id,
          extension: s.extension,
          bannerSetted: s.bannerSetted,
          title: title,
          brief: getLocalForVal(s.brief, s.briefLocal, locale),
        });
        const keys = title.split(' ');
        keyWords.push(...keys);
      });
    })
    .catch((error) => {});

  return {
    props: {
      news,
      translations,
      keyWords,
    },
  };
}
