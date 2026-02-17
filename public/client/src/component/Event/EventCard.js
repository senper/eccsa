import React from "react";
import {
  Grid,
  CardContent,
  CardMedia,
  CardHeader,
  Typography,
  Card,
} from "@material-ui/core";
import EventTimeline from "./EventTimeline";

export default function EventCard({event}) {
  return (
    <Grid item xs={12} >
        <Card >
        <CardHeader
            title={event.title}
            subheader={`${event.startDateStr} - ${event.endDateStr}`}
        />
        <CardMedia
            component="img"
            height="300"
            image={`${
            process.env.REACT_APP_API_URL
            }/${event.ownerStr.toLowerCase()}/documents/${event.id
            .substr(event.id.length - 15)
            .replace("-", "")}.jpg`}
            title={event.title}
        />
        <CardContent >
            <Typography variant="body2" >
            {event.description}{" "}
            </Typography>
            <EventTimeline selectedEvent={event}/>
        </CardContent>
        </Card>
    </Grid>
  );
}
