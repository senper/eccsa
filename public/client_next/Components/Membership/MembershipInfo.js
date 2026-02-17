import React from "react";
import { getMembershipInfo, getApplications } from "../../api/membership";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ExistingMemberships from "./ExistingMemberships";
import ApplyForMembership from "./ApplyForMembership";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import Link from "next/link";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
} from "@mui/material";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { Card, Col, Row } from "react-bootstrap";

export default function MembershipInfo({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
  const [memberships, setMemberships] = React.useState([]);
  const [applications, setApplications] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    setLoading(true);
    getMembershipInfo()
      .then((data) => {
        setMemberships(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });

    getApplications().then((data) => {
      setApplications(data.data.$values);
    });
  }, []);

  const columns = [
    {
      headerName: getLocal("Apply Date"),
      field: "dateApplied",
      flex: 0.1,
    },
    {
      headerName: `${getLocal("Camber")}/${getLocal("Association")}`,
      field: "associationName",
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Type"),
      field: "applicationType",
      flex: 0.1,
    },
    {
      headerName: getLocal("Status"),
      field: "applicationStatus",
      flex: 0.1,
    },
    {
      headerName: getLocal("Documents Status"),
      field: "totalDocumentsNo",
      valueGetter: (params) => {
        const rejectedCount = params.row.rejectedDocumentsNo;
        const acceptedCount = params.row.acceptedDocumentsNo;
        return `${rejectedCount} ${getLocal(
          "Rejected"
        )}; ${acceptedCount} ${getLocal("Accepted")} ${getLocal("of")} ${
          params.value
        }`;
      },
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.05,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            href={`/myServices/applicationDetailPage?applicationId=${params.value}&associationId=${params.row.associationId}`}
            legacyBehavior>
            {getLocal("Detail")}
          </Link>
        </div>
      ),
    },
  ];
  return <>
    <Grid item xs={12}>
      <hr style={{ width: "100%", marginTop: "2px" }} />
      <h5>
        <p className="links" style={{ margin: "0px", textAlign: "left" }}>
          {getLocal("Existing")} {getLocal("Memberships")}
        </p>
      </h5>
      {loading ? <h3>{getLocal("Loading")}...</h3> : null}
      <ExistingMemberships
        loading={loading}
        memberships={memberships}
        getLocal={getLocal}
        getLocalByVal={getLocalByVal}
      />
      <br />
      <ApplyForMembership
        memberships={memberships}
        getLocal={getLocal}
        getLocalByVal={getLocalByVal}
      />
      <Accordion defaultExpanded={expanded}>
        <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links">
              {" "}
              {getLocal("Membership")} {getLocal("Requests")}
            </p>
          </div>
        </AccordionSummary>
        <AccordionDetails style={{ display: "block" }}>
          <>
            <div id="showOnLargeScreen">
              <CustomDataGrid dataRows={applications} columns={columns} />
            </div>
            <div id="showOnSmallScreen">
              {applications && applications.length > 0 ? (
                applications.map((r, i) => (
                  <Card key={i}>
                    <Card.Header>
                      {r.associationName}-{r.dateApplied}
                    </Card.Header>
                    <Card.Body>
                      <Row>
                        <Col>{getLocal("Apply Date")}</Col>
                        <Col>{r.dateApplied}</Col>
                      </Row>
                      <Row>
                        <Col>
                          {getLocal("Camber")}/{getLocal("Association")}
                        </Col>
                        <Col>{r.associationName}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Type")}</Col>
                        <Col>{r.applicationType}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Status")}</Col>
                        <Col>{r.applicationStatus}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Documents Status")}</Col>
                        <Col>{`${r.rejectedDocumentsNo} ${getLocal(
                          "Rejected"
                        )}; ${r.acceptedDocumentsNo} ${getLocal(
                          "Accepted"
                        )} ${getLocal("of")} ${r.totalDocumentsNo}`}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Actions")}</Col>
                        <Col>
                          <Link
                            href={`/myServices/applicationDetailPage?applicationId=${r.id}&associationId=${r.associationId}`}
                            legacyBehavior>
                            {getLocal("Detail")}
                          </Link>
                        </Col>
                      </Row>
                    </Card.Body>
                  </Card>
                ))
              ) : (
                <>No Membership Application(s) Found</>
              )}
            </div>
          </>
        </AccordionDetails>
      </Accordion>
    </Grid>
  </>;
}
