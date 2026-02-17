import {
  Card,
  CardContent,
  CardHeader,
  CardMedia,
  Grid,
  Typography,
} from "@mui/material";
import React from "react";
import EventTimeline from "./EventTimeline";

export default function EventCard({ event, getLocal, getLocalByVal }) {
  return (
    <Grid item xs={12}>
      <Card>
        <CardHeader
          title={getLocalByVal(event.title, event.titleLocal)}
          subheader={`${event.startDateStr} - ${event.endDateStr}`}
        />
        <CardMedia
          component="img"
          height="300"
          image={`${
            process.env.NEXT_PUBLIC_API_URL
          }/${event.ownerStr.toLowerCase()}/documents/${event.id
            .substr(event.id.length - 15)
            .replace("-", "")}.jpg`}
          title={getLocalByVal(event.title, event.titleLocal)}
        />
        <CardContent>
          <Typography variant="body2">
            {getLocalByVal(event.description, event.descriptionLocal)}{" "}
          </Typography>
          <EventTimeline
            selectedEvent={event}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </CardContent>
      </Card>
    </Grid>
  );
}
