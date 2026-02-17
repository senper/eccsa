import { Grid } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";
import { getEvents } from "../../services/event";
import EventsHighlight from "./EventsHighlight";
import EventTimeline from "./EventTimeline";
function EventsSection() {
  const [selectedEvent, setSelectedEvent ] = React.useState(null);
  const [events, setEvents] = React.useState([]);
  React.useEffect(() => {
    getEvents().then((data) => {
      setEvents(data.data.$values);
      if(data.data.$values && data.data.$values.length > 0){
        setSelectedEvent(data.data.$values[0]);
      }
    });
  }, []);
  return (
    <>
      {selectedEvent ? (<><Grid item xs={12} md={6}>
        <div className="row">
          <div className="bg">Events</div>
        </div>
        <EventsHighlight setSelectedEvent={setSelectedEvent} events={events}  />
        <Link className="btn btn-success"
        to={{
          pathname: "/events",
          state: {
            eventKey: "events",
          },
        }}>
          {" "}
          More Events
        </Link>
      </Grid>
      <Grid item xs={12} md={6}>
        <div className="row">
          <div className="bg">{selectedEvent ? selectedEvent.title : ""} Event Details</div>
        </div>
        <EventTimeline selectedEvent={selectedEvent}/>
      </Grid></>) : null}
    </>
  );
}

export default EventsSection;
