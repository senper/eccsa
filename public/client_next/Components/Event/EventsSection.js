import { Grid } from "@mui/material";
import Link from "next/link";
import React from "react";
import { getEvents } from "../../api/event";
import EventsHighlight from "./EventsHighlight";
import EventTimeline from "./EventTimeline";
export default function EventsSection({ getLocal, getLocalByVal }) {
  const [selectedEvent, setSelectedEvent] = React.useState(null);
  const [events, setEvents] = React.useState([]);
  React.useEffect(() => {
    getEvents().then((data) => {
      setEvents(data.data.$values);
      if (data.data.$values && data.data.$values.length > 0) {
        setSelectedEvent(data.data.$values[0]);
      }
    });
  }, []);
  return <>
    {selectedEvent ? (
      <>
        <Grid item xs={12} md={6}>
          <div className="bg">{getLocal("Events")}</div>
          <EventsHighlight
            setSelectedEvent={setSelectedEvent}
            events={events}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
          <Link href={"/events"} className="btn btn-success">

            {getLocal("More")} {getLocal("Events")}

          </Link>
        </Grid>
        <Grid item xs={12} md={6}>
          <div className="bg">
            {selectedEvent ? getLocalByVal(selectedEvent.title, selectedEvent.titleLocal) : ""}{" "}
            {getLocal("Event Details")}
          </div>
          <EventTimeline
            selectedEvent={selectedEvent}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
      </>
    ) : null}
  </>;
}
