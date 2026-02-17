import React from 'react';
import { Grid, Container, Breadcrumb, Link } from '@mui/material';
import BusinessInfoNewsCard from '../Components/BusinessInformation/BusinessInfoNewsCard';
import {
  getBusinessInformation,
  getBusinessInformationList,
  getPastBusinessInformationList,
} from '../api/businessInformation';
import { useRouter } from 'next/router';
import { getLocalByKey } from './api/menu';
import { CommonBreadcrumb } from '../Components/Layout/CommonBreadcrumb';

export default function BusinessInformation({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  let newsIdValue = 0;
  if (location.query) {
    const { newsId } = location.query;
    newsIdValue = newsId;
  }
  const [loadingCurr, setLoadingCurr] = React.useState(false);
  const [loadingPast, setLoadingPast] = React.useState(false);
  const [news, setNews] = React.useState([]);
  React.useEffect(() => {
    setLoadingCurr(true);
    getBusinessInformationList()
      .then((data) => {
        setNews(data.data.$values);
        setLoadingCurr(false);
      })
      .catch((error) => {
        setLoadingCurr(false);
      });
  }, []);
  const [pastNews, setPastNews] = React.useState([]);
  React.useEffect(() => {
    setLoadingPast(true);
    getPastBusinessInformationList()
      .then((data) => {
        setPastNews(data.data.$values);
        setLoadingPast(false);
      })
      .catch((error) => {
        setLoadingPast(false);
      });
  }, []);
  const [selectedPastNews, setSelectedPastNews] = React.useState(null);
  const loadNews = (value) => {
    getBusinessInformation(value.id).then((data) => {
      setSelectedPastNews(data.data);
    });
  };

  function compare2(a, b) {
    if (newsIdValue === a.id) {
      return -1;
    }
    if (newsIdValue === b.id) {
      return 1;
    }
    return 0;
  }

  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal(' Business Information'),
      path: '#',
    },
  ];
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2} className="grid">
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Container>
            <div className="bg" style={{ marginTop: '0' }}>
              {!loadingCurr && news.length <= 0
                ? getLocal('No Current Business Information Found')
                : getLocal('Current Business Information')}
            </div>
            <Grid container spacing={1}>
              {loadingCurr ? <h5>{getLocal('loading')} ...</h5> : null}

              {news.sort(compare2).map((value, index) => (
                <BusinessInfoNewsCard
                  news={value}
                  getLocal={getLocal}
                  getLocalByVal={getLocalByVal}
                  key={index}
                />
              ))}
            </Grid>
            <div className="bg">
              {!loadingPast && pastNews.length <= 0
                ? getLocal('No past Business Information Found')
                : getLocal('Past Business Information')}
            </div>
            <Grid container spacing={1}>
              {selectedPastNews ? (
                <>
                  <BusinessInfoNewsCard
                    news={selectedPastNews}
                    getLocal={getLocal}
                    getLocalByVal={getLocalByVal}
                  />
                  <Grid item xs={12}>
                    <a
                      className="btn btn-sm btn-warning"
                      onClick={() => setSelectedPastNews(null)}
                    >
                      {getLocal('Close')}
                    </a>
                  </Grid>
                </>
              ) : null}
              <Grid item xs={12} container>
                {loadingPast ? <h5>{getLocal('loading')} ...</h5> : null}
                {pastNews.map((value) => (
                  <div key={value.id}>
                    <Link onClick={() => loadNews(value)}>
                      {getLocalByVal(value.title, value.titleLocal)}
                    </Link>
                    &nbsp;&nbsp;&nbsp;{'  '}
                  </div>
                ))}
              </Grid>
            </Grid>
          </Container>
        </Grid>
      </Grid>
    </>
  );
}
