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

export default function ResourceCard({ resource, showRequest=true }) {
  const formateProfile = () => {
    let profile = resource.profile;
    profile = profile.replace(
      'src="/Resource/photo/img',
      `src="${process.env.REACT_APP_API_URL}/Resource/photo/img`
    );
    return profile;
  };
  const getAttachmentLink = () => {
    if (
      !resource.extension ||
      resource.extension.trim() == "" ||
      !resource.hasFile
    )
      return null;
    const fileName = resource.id
      .substr(resource.id.length - 15)
      .replace("-", "");
    const moduleUrl =
      resource.ownerModule == "Communication"
        ? "pr"
        : resource.ownerModule.toLowerCase();
    const relativePath = `${moduleUrl}/documents/${fileName}${resource.extension}`;

    return (
      <a
        href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
        target="_blank"
        rel="noreferrer"
        title={resource.title + " Attachment"}
        download={resource.title + " Attachment" + resource.extension}
      >
        Click To Download Resource
      </a>
    );
  };
  return (
    <>
      <style>{`ul {padding: 0px;}`}</style>
      <Grid item xs={12}>
        <Card>
          <CardHeader
            title={resource.title}
            subheader={
              <>
                Author: {resource.author}, Publication Date:{" "}
                {resource.publicationDateStr}
              </>
            }
          />
          <CardContent>
            <Typography variant="body2">
              <strong>Description</strong>: <br />
              <pre>{resource.description}</pre>
            </Typography>

            {resource.printUri ||
            (resource.printCost && parseFloat(resource.printCost) > 0) ? (
              <Row>
                {resource.printUri ? (
                  <>
                    <Col>
                      Print Uri: <strong>{resource.printUri}</strong>
                    </Col>
                  </>
                ) : null}
                {resource.printCost && parseFloat(resource.printCost) > 0 ? (
                  <>
                    <Col md={4}>
                      {" "}
                      Print Cost: <strong>{resource.printCost}</strong>
                    </Col>
                  </>
                ) : null}
              </Row>
            ) : null}

            {resource.digitalCost <= 0 ? (
              <Row>
                <Col>{getAttachmentLink()}</Col>
              </Row>
            ) : null}
            {resource.digitalCost <= 0 && resource.url ? (
              <Row>
                <Col>
                  <a target="_blank" href={resource.url}>
                    Go To Link
                  </a>{" "}
                </Col>
              </Row>
            ) : null}
            {showRequest && resource.digitalCost > 0.01e-6 ? (
              <Link
                class="btn btn-success"
                style={{float: "left"}}
                to={{
                  pathname: "/serviceRequestForm",
                  state: {
                    serviceTypeId: resource.id,
                  },
                }}
              >
                {" "}
                Request
              </Link>
            ) : null}

            {resource.profile ? (
              <>
                <strong style={{ paddingLeft: "10px", fontSize: "16px" }}>
                  Profile
                </strong>
                <br />
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
