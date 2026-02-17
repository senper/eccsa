import React from "react";
import {
  Grid,
  CardContent,
  CardMedia,
  CardHeader,
  Typography,
  Card,
} from "@material-ui/core";
import { Link } from "react-router-dom";

export default function BusinessInfoNewsCard({ news }) {
  return (
    <>
    <style>{`ul {padding: 0px;}`}</style>
    <Grid item xs={12}>
      <Card>
        <CardHeader
          title={news.title}
          subheader={
            <>
              {news.link ? (
                <Link
                  target="_blank"
                  to={{
                    pathname: "#",
                    state: {
                      eventKey: "events",
                    },
                  }}
                >
                  {news.link}
                </Link>
              ) : null}
            </>
          }
        />
        <CardContent>
          <Typography variant="body2"><pre>{news.description}</pre></Typography>
        </CardContent>
      </Card>
    </Grid></>
  );
}
