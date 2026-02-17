import React from "react";
import { Breadcrumbs, Grid, Container } from "@material-ui/core";
import { Link } from "react-router-dom";
import { getEvents, getPastEvents, getEvent } from "../../services/event";
import Footer from "../Footer/Footer";
import EventCard from "./EventCard";
import { SignalCellularNullSharp } from "@material-ui/icons";
import { makeStyles } from "@material-ui/core/styles";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));
function App() {
  const classes = useStyle();
  const [loadingCurr, setLoadingCurr] = React.useState(false);
  const [loadingPast, setLoadingPast] = React.useState(false);
  const [events, setEvents] = React.useState([]);
  React.useEffect(() => {
    setLoadingCurr(true);
    getEvents()
      .then((data) => {
        setEvents(data.data.$values);
        setLoadingCurr(false);
      })
      .catch((error) => {
        setLoadingCurr(false);
      });
  }, []);
  const [pastEvents, setPastEvents] = React.useState([]);
  React.useEffect(() => {
    setLoadingPast(true);
    getPastEvents()
      .then((data) => {
        setPastEvents(data.data.$values);
        setLoadingPast(false);
      })
      .catch((error) => {
        setLoadingPast(false);
      });
  }, []);
  const [selectedPastEvent, setSelectedPastEvent] = React.useState(null);
  const loadEvent = (id) => {
    getEvent(id).then((data) => {
      setSelectedPastEvent(data.data);
    });
  };
  return (
    <div class="mmenu">
      <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
        <Link
          color="inherit"
          to={{
            pathname: "/",
            state: {
              eventKey: "default",
            },
          }}
        >
          Home
        </Link>
        <Link color="textPrimary" to="#" aria-current="page">
          Events
        </Link>
      </Breadcrumbs>
      <Grid container spacing={2} className="grid">
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={12}>
          <Container>
            <div className="bg" style={{ marginTop: "0" }}>
              {!loadingCurr && (!events || events.length <= 0)
                ? "No Current and Upcoming Events Found"
                : "Current and Upcoming Events"}
            </div>
            <Grid container spacing={1}>
              {loadingCurr ? <h5>loading ...</h5> : null}
              {events
                ? events.map((value) => <EventCard event={value} />)
                : SignalCellularNullSharp}
            </Grid>
            <div className="bg">
              {!loadingPast && (!pastEvents || pastEvents.length <= 0)
                ? "No Past Events Found"
                : "Past Events"}
            </div>
            <Grid container spacing={1}>
              {loadingPast ? <h5>loading ...</h5> : null}
              {selectedPastEvent ? (
                <>
                  <EventCard event={selectedPastEvent} />
                  <Grid item xs={12}>
                    <Link
                      style={{ margin: "0px 0px 0px 0px" }}
                      className="btn btn-sm btn-warning"
                      onClick={() => setSelectedPastEvent(null)}
                    >
                      Close
                    </Link>
                  </Grid>
                </>
              ) : null}
              <Grid item xs={12}>
                {pastEvents
                  ? pastEvents.map((value) => (
                      <>
                        <Link onClick={() => loadEvent(value.id)}>
                          {value.title}
                        </Link>
                        &nbsp;&nbsp;&nbsp;{"  "}
                      </>
                    ))
                  : null}
              </Grid>
            </Grid>
          </Container>
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
