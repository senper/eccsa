import { Grid, Paper } from "@mui/material";
import { useRouter } from "next/router";
import React from "react";
import { Card } from "react-bootstrap";
import {
  getResources /* resourceTypes */,
  getResourcesByTypeId,
} from "../../api/resources";
import { CommonBreadcrumb } from "../../Components/Layout/CommonBreadcrumb";
import ResourceCard from "../../Components/Resources/ResourceCard";
import ResourceLinks from "../../Components/Resources/ResourceLinks";
import Custom404 from "../404";
import { getLocalByKey, getPathFromTitle } from "../api/menu";
import { getLookupValues } from "../api/contactUs";

export default function EResources({
  translations,
  serverResources,
  keyWords,
  setAdditionalKeywords,
  setPageTitle,
  resourceTypes,
  typeIdValue,
  typeTextValue,
}) {
  const location = useRouter();
  const [resources, setResources] = React.useState([]);
  const [resourceType, setResourceType] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const { locale } = location;
  const { type } = location.query;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };

  React.useEffect(() => {
    if (typeIdValue) {
      setLoading(true);
      setResources([]);
      setResourceType(typeTextValue);
      setResources(serverResources);
      setLoading(false);
      setAdditionalKeywords(keyWords);
      setPageTitle("EResources");
    }
  }, [typeIdValue, keyWords]);

  const [selectedResource, setSelectedResource] = React.useState(null);
  const loadResource = (value) => {
    if (selectedResource && value.id == selectedResource.id) {
      setSelectedResource(null);
      return;
    }
    setSelectedResource(value);
    /* getResource(value.id).then((data) => {
      setSelectedResource(data.data);
    }); */
  };
  const breadcrumbLinks = () => [
    {
      title: getLocal("Home"),
      path: "/",
    },
    {
      title: getLocal("e-Resources"),
      path: "/eresources",
    },
  ];

  if (type && !typeIdValue) {
    return <Custom404 />;
  }
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2}>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Paper style={{ padding: "6px" }}>
            <Card>
              <Card.Body>
                <h3>{typeIdValue ? resourceType : null}</h3>
                <ol>
                  {typeIdValue ? (
                    <>
                      {loading ? <h3>{getLocal("Loading")}...</h3> : null}
                      {!loading && resources && resources.length > 0
                        ? resources.map((value, index) => (
                            <li key={index}>
                              <a
                                
                                style={{ cursor: "pointer", color: "#0d6efd", textDecoration: "underline" }}
                                onClick={() => loadResource(value)}
                              >
                                <strong>
                                  {getLocalByVal(value.title, value.titleLocal)}
                                </strong>
                              </a>
                              {selectedResource &&
                              selectedResource.id === value.id ? (
                                <>
                                  <ResourceCard
                                    resource={selectedResource}
                                    getLocal={getLocal}
                                    getLocalByVal={getLocalByVal}
                                  />
                                  <a
                                    style={{
                                      margin: "0px 0px 0px 0px",
                                      cursor: "pointer",
                                    }}
                                    className="btn btn-sm btn-warning"
                                    onClick={() => setSelectedResource(null)}
                                  >
                                    {getLocal("Close")}
                                  </a>
                                  <br />
                                </>
                              ) : null}
                            </li>
                          ))
                        : null}
                      {!loading && (!resources || resources.length <= 0) ? (
                        <strong>{getLocal("No Resources Found")}</strong>
                      ) : null}
                    </>
                  ) : (
                    <>
                      <img src="/images/resourceimg1.png" alt="resourceimg1" />
                      <br />
                      <img src="/images/resourceimg2.png" alt="resourceimg2" />
                    </>
                  )}
                </ol>
              </Card.Body>
            </Card>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <ResourceLinks
            resourceTypes={resourceTypes}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
      </Grid>
    </>
  );
}

export async function getServerSideProps(context) {
  const { type } = context.query;
  let typeIdValue = null;
  let serverResources = [];
  let resourceTypes = [];
  let typeTextValue = "";
  let selectedType = null;
  await getLookupValues("00000000-0000-0000-0000-000000000024").then((data) => {
    resourceTypes = data.data.$values.filter(l => l.value != 0);
  });

  if (type && type.length == 1) {
    selectedType = resourceTypes.find(
      (t) => getPathFromTitle(t.name) == type[0].toLowerCase()
    );
  }
  if (selectedType) {
    typeIdValue = selectedType.id;
    typeTextValue = selectedType.name;
    await getResourcesByTypeId(typeIdValue).then((data) => {
      serverResources = data.data.$values;
    });
  }

  return {
    props: {
      serverResources,
      resourceTypes,
      typeIdValue,
      typeTextValue,
    },
  };
}
