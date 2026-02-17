import { Card, CardContent, Container, Grid, Typography } from '@mui/material';
import { useRouter } from 'next/router';
import React from 'react';
import { getNewsById } from '../../api/news';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';
import styles from '../../styles/News.module.css';
import { getLocalByKey, getLocalForVal, getTranslations } from '../api/menu';

export default function NewsDetail({
  news,
  translations,
  keyWords,
  setAdditionalKeywords,
  setPageTitle,
}) {
  window.scrollTo({
    top: 0,
    behavior: 'smooth',
  });
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
      path: '/news',
    },
    {
      title: getLocal('New Detail'),
      path: '#',
    },
  ];
  React.useEffect(() => {
    setAdditionalKeywords(keyWords);
    setPageTitle('News Details');
  }, [keyWords]);

  const formateProfile = () => {
    let profile = news.full;
    profile = profile.replaceAll(
      'src="/pr/news',
      `src="${process.env.NEXT_PUBLIC_API_URL}/pr/news`
    );
    return profile;
  };
  const imgurl = () => {
    if (news.homeBannerSetted) {
      return `${process.env.NEXT_PUBLIC_API_URL}\\${news.homeBannerUrlRelative}`;
    } else {
      return `${process.env.NEXT_PUBLIC_API_URL}\\${news.bannerUrlRelative}`;
    }
  };
  return (
    <main>
      {<style>{`h6{fontStyle: "italic", marginTop:"10px"}`}</style>}

      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2} className={styles.cardGrid}>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={9}>
          <Container>
            {news ? (
              <Card className={styles.card}>
                <Typography
                  gutterBottom
                  variant="h6"
                  className={styles.titledetail}
                >
                  {news.title}
                </Typography>
                <CardContent className={styles.cardContent}>
                  {news && news.bannerSetted ? (
                    <img
                      src={imgurl()}
                      width="100%"
                      style={{ maxWidth: '900px' }}
                      alt={news.title}
                    />
                  ) : null}
                  <div dangerouslySetInnerHTML={{ __html: formateProfile() }} />
                  <h6 style={{ fontStyle: 'italic', marginTop: '10px' }}>
                    {getLocal('Author')}: {news.author}
                    <br />
                    {news.newsDate.substr(0, 10)}
                  </h6>
                </CardContent>
              </Card>
            ) : null}
          </Container>
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
  const { news_id } = context.query;
  let news = null;
  let translations = [];
  await getTranslations()
    .then((data) => {
      translations = data.data.$values;
    })
    .catch((error) => {});
  await getNewsById(news_id)
    .then((data) => {
      let s = data.data;
      news = {
        id: s.id,
        extension: s.extension,
        bannerUrlRelative: s.bannerUrlRelative,
        bannerSetted: s.bannerSetted,
        homeBannerUrlRelative: s.homeBannerUrlRelative,
        homeBannerSetted: s.homeBannerSetted,
        newsDate: s.newsDate,
        title: getLocalForVal(s.title, s.titleLocal, locale),
        author: getLocalForVal(s.author, s.authorLocal, locale),
        full: getLocalForVal(s.full, s.fullLocal, locale),
      };
    })
    .catch((error) => {});

  return {
    props: {
      news,
      translations,
    },
  };
}
