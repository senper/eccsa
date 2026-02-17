import { Card, CardContent, CardHeader, Grid, Typography } from "@mui/material";
import Link from "next/link";
import React from "react";
import { Col, Row } from "react-bootstrap";

export default function ResourceCard({
  resource,
  showRequest = true,
  getLocal,
  getLocalByVal,
}) {
  if(!resource) return null;
  const isValidUrl = urlString =>{
    var inputElement = document.createElement('input',);
    inputElement.type = 'url';
    inputElement.value = urlString;

    if (!inputElement.checkValidity()) {
      inputElement.remove();
      return false;
    } else {
      inputElement.remove();
      return true;
    }
  } 
  const formateProfile = () => {
    let profile = getLocalByVal(resource.profile, resource.profileLocal);
    profile = profile.replaceAll(
      'src="/Resource/photo/img',
      `src="${process.env.NEXT_PUBLIC_API_URL}/Resource/photo/img`
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
        href={`${process.env.NEXT_PUBLIC_API_URL}/${relativePath}`}
        target="_blank"
        rel="noreferrer"
        title={resource.title + " Attachment"}
        download={resource.title + " Attachment" + resource.extension}
      >
        {getLocal("Click To Download Resource")}
      </a>
    );
  };
  return <>
    <style>{`ul {padding: 0px;}`}</style>
    <Grid item xs={12}>
      <Card>
        <CardHeader
          title={getLocalByVal(resource.title, resource.titleLocal)}
          subheader={
            <>
              {getLocal("Author")}:{" "}
              {getLocalByVal(resource.author, resource.authorLocal)},{" "}
              {getLocal("Publication Date")}: {resource.publicationDateStr}
            </>
          }
        />
        <CardContent>
          <Typography variant="body2">
            <strong>{getLocal("Description")}</strong>: <br />
          </Typography>
          <pre>
            {getLocalByVal(resource.description, resource.descriptionLocal)}
          </pre>

          {resource.printUri ||
          (resource.printCost && parseFloat(resource.printCost) > 0) ? (
            <Row>
              {resource.printUri ? (
                <>
                  <Col>
                    {getLocal("Print Uri")}:{" "}
                    <strong>{resource.printUri}</strong>
                  </Col>
                </>
              ) : null}
              {resource.printCost && parseFloat(resource.printCost) > 0 ? (
                <>
                  <Col md={4}>
                    {" "}
                    {getLocal("Print Cost")}:{" "}
                    <strong>{resource.printCost}</strong>
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
          {resource.digitalCost <= 0 && resource.url && isValidUrl(resource.url) ? (
            <Row>
              <Col>
                <a target="_blank" href={resource.url} rel="noreferrer">
                  {getLocal("Go To Link")}
                </a>{" "}
              </Col>
            </Row>
          ) : null}
          {showRequest && resource.digitalCost > 0.01e-6 ? (
            (<Link
              href={`/myServices/serviceRequestForm?serviceTypeId=${resource.id}`}
              className="btn btn-success"
              style={{ float: "left" }}>

              {getLocal("Request")}

            </Link>)
          ) : null}

          {resource.profile ? (
            <>
              <strong style={{ paddingLeft: "10px", fontSize: "16px" }}>
                {getLocal("Profile")}
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
  </>;
}
