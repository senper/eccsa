import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import Link from "next/link";
import { Card, Col, Form, InputGroup, Row } from "react-bootstrap";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import React from "react";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileArchive } from "@fortawesome/free-solid-svg-icons";
import { getResourceRequests } from "../../api/serviceRequest";

export default function ResourceRequestsList({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
  const [serviceRequests, setServiceRequests] = React.useState([]);
  const [filterStatus, setFilterStatus] = React.useState(1);
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    loadList(filterStatus);
  }, []);

  const loadList = (status) => {
    setLoading(true);
    getResourceRequests(status)
      .then((data) => {
        setServiceRequests(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  };

  const columns = [
    {
      headerName: getLocal("Requested on"),
      field: "requestDateStr",
      flex: 0.2,
    },
    {
      headerName: getLocal("Resource Name"),
      field: "serviceType",
      renderCell: renderCellExpand,
      valueGetter: (params) => {
        return params.value
          ? getLocalByVal(params.value.nameEng, params.value.nameLocal)
          : null;
      },
      flex: 0.2,
    },
    {
      headerName: getLocal("Status"),
      field: "requestStatus",
      flex: 0.2,
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            href={`/myServices/resourceRequestDetailPage?serviceRequestId=${params.value}&serviceTypeId=${params.row.serviceTypeId}`}
            legacyBehavior>
            {getLocal("Detail")}
          </Link>
        </div>
      ),
    },
  ];
  return <>
    <Grid item xs={12}>
      <Accordion defaultExpanded={expanded}>
        <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
          <div className="icon">
            <FontAwesomeIcon icon={faFileArchive} size="1x" />
          </div>
          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links">
              {" "}
              {getLocal("Requested")} {getLocal("Resources")}
            </p>
          </div>
        </AccordionSummary>
        <AccordionDetails style={{ display: "block" }}>
          <Row>
            <Col md={4} sm={6}>
              <InputGroup>
                <InputGroup.Text>{getLocal("Filter Status")}</InputGroup.Text>
                <Form.Select
                  value={filterStatus}
                  onChange={(e) => {
                    setFilterStatus(e.target.value);
                    loadList(e.target.value);
                  }}
                >
                  <option value="1">Active</option>
                  <option value="0">Closed</option>
                </Form.Select>
              </InputGroup>
            </Col>
          </Row>
          {loading ? (
            <h2 className="section-title">
              <br />
              {getLocal("Loading Requests list")}...
            </h2>
          ) : (
            <>
              <div id="showOnLargeScreen">
                <CustomDataGrid
                  dataRows={serviceRequests}
                  columns={columns}
                />
              </div>
              <div id="showOnSmallScreen">
                {serviceRequests && serviceRequests.length > 0 ? (
                  serviceRequests.map((r, i) => (
                    <Card key={i}>
                      <Card.Header>
                        {r.serviceType
                          ? getLocalByVal(
                              r.serviceType.nameEng,
                              r.serviceType.nameLocal
                            )
                          : null}
                        -{r.requestDateStr}
                      </Card.Header>
                      <Card.Body>
                        <Row>
                          <Col>{getLocal("Requested on")}</Col>
                          <Col>{r.requestDateStr}</Col>
                        </Row>
                        <Row>
                          <Col>{getLocal("Status")}</Col>
                          <Col>{r.requestStatus}</Col>
                        </Row>
                        <Row>
                          <Col>{getLocal("Actions")}</Col>
                          <Col>
                            <Link
                              href={`/myServices/resourceRequestDetailPage?serviceRequestId=${r.id}&serviceTypeId=${r.serviceTypeId}`}
                              legacyBehavior>
                              {getLocal("Detail")}
                            </Link>
                          </Col>
                        </Row>
                      </Card.Body>
                    </Card>
                  ))
                ) : (
                  <>No Requests Found</>
                )}
              </div>
            </>
          )}
        </AccordionDetails>
      </Accordion>
    </Grid>
  </>;
}
