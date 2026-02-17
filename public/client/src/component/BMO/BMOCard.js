import React from "react";
import {
  Grid,
  CardContent,
  CardHeader,
  Typography,
  Card,
} from "@material-ui/core";
import { Col, Row } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function BMOCard({ bmo, showRequest=true }) {
  const formateProfile = () => {
    let profile = bmo.content;
    profile = profile.replace(
      'src="/pr/text_media',
      `src="${process.env.REACT_APP_API_URL}/pr/text_media`
    );
    return profile;
  };
  
  return (
    <>
      <style>{`ul {padding: 0px;}`}</style>
      <Grid item xs={12}>
        <Card>
          <CardContent>
            {bmo.content ? (
              <>
                <div
                  style={{
                    border: "1px solid #ccc",
                    borderRadius: "16px",
                    padding: "0.01em 16px",
                  }}
                  dangerouslySetInnerHTML={{ __html: formateProfile() }}
                />
              </>
            ) : null}
          </CardContent>
        </Card>
      </Grid>
    </>
  );
}
