import { SignalCellularNullSharp } from '@mui/icons-material';
import { Grid } from '@mui/material';
import { Container } from '@mui/system';
import { useRouter } from 'next/router';
import React from 'react';
import { Button } from 'react-bootstrap';
import { getEvent, getEvents, getPastEvents } from '../api/event';
import EventCard from '../Components/Event/EventCard';
import { CommonBreadcrumb } from '../Components/Layout/CommonBreadcrumb';
import { getLocalByKey } from './api/menu';

export default function Events({ translations, prevEvents, currEvents }) {
  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };
  const [loadingCurr, setLoadingCurr] = React.useState(false);
  const [loadingPast, setLoadingPast] = React.useState(false);
  const [events, setEvents] = React.useState([]);
  React.useEffect(() => {
    setLoadingCurr(true);

    setEvents(currEvents);
    setLoadingCurr(false);

    setLoadingCurr(false);
  }, []);
  const [pastEvents, setPastEvents] = React.useState([]);
  React.useEffect(() => {
    setLoadingPast(true);

    setPastEvents(prevEvents);
    setLoadingPast(false);

    setLoadingPast(false);
  }, []);
  const [selectedPastEvent, setSelectedPastEvent] = React.useState(null);
  const loadEvent = (id) => {
    getEvent(id).then((data) => {
      setSelectedPastEvent(data.data);
    });
  };

  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Events'),
      path: '#',
    },
  ];
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2}>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={12}>
          <Container>
            <div className="bg" style={{ marginTop: '0' }}>
              {!loadingCurr && (!events || events.length <= 0)
                ? getLocal('No Current and Upcoming Events Found')
                : getLocal('Current and Upcoming Events')}
            </div>
            <Grid container spacing={1}>
              {loadingCurr ? <h5>{getLocal('loading')} ...</h5> : null}
              {events
                ? events.map((value, index) => (
                    <EventCard
                      key={index}
                      event={value}
                      getLocal={getLocal}
                      getLocalByVal={getLocalByVal}
                    />
                  ))
                : SignalCellularNullSharp}
            </Grid>
            <div className="bg">
              {!loadingPast && (!pastEvents || pastEvents.length <= 0)
                ? getLocal('No Past Events Found')
                : getLocal('Past Events')}
            </div>
            <Grid container spacing={1}>
              {loadingPast ? <h5>{getLocal('loading')} ...</h5> : null}
              {selectedPastEvent ? (
                <>
                  <EventCard
                    event={selectedPastEvent}
                    getLocal={getLocal}
                    getLocalByVal={getLocalByVal}
                  />
                  <Grid item xs={12}>
                    <Button
                      style={{ margin: '0px 0px 0px 0px' }}
                      className="btn btn-sm btn-warning"
                      onClick={() => setSelectedPastEvent(null)}
                    >
                      {getLocal('Close')}
                    </Button>
                  </Grid>
                </>
              ) : null}
              <Grid item xs={12}>
                {pastEvents
                  ? pastEvents.map((value, index) => (
                      <span key={index} style={{ display: '' }}>
                        <a
                          className="text-primary"
                          style={{ cursor: 'pointer' }}
                          onClick={() => loadEvent(value.id)}
                        >
                          {getLocalByVal(value.title, value.titleLocal)}
                        </a>
                        &nbsp;&nbsp;&nbsp;{'  '}
                      </span>
                    ))
                  : null}
              </Grid>
            </Grid>
          </Container>
        </Grid>
      </Grid>
    </>
  );
}

export async function getServerSideProps(context) {
  let prevEvents = [];
  let currEvents = [];

  await getPastEvents().then((data) => {
    prevEvents = data.data.$values;
  });

  await getEvents().then((data) => {
    currEvents = data.data.$values;
  });

  return {
    props: {
      prevEvents,
      currEvents,
    },
  };
}
