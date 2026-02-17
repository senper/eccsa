import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { getServiceRequests } from "../../services/serviceRequest";
import { Col, Form, InputGroup, Row } from "react-bootstrap";
import { CertificateOfOriginId } from "../../services/servicetype";

export default function ServiceRequestsList({ expanded = true }) {
  const token = localStorage.getItem("access_token");
  const [serviceRequests, setServiceRequests] = React.useState([]);
  const [filterStatus, setFilterStatus] = React.useState(1);
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    if (token) {
      loadList(filterStatus);
    }
  }, []);

  const loadList = (status) => {
    setLoading(true);
    getServiceRequests(status)
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
      headerName: "Requested on",
      field: "requestDateStr",
      flex: 0.2,
    },
    {
      headerName: "Service Name",
      field: "serviceType",
      renderCell: renderCellExpand,
      valueGetter: (params) => {
        return params.value ? params.value.name : null;
      },
      flex: 0.2,
    },
    {
      headerName: "Status",
      field: "requestStatus",
      flex: 0.2,
    },
    {
      headerName: "Documents Status",
      field: "totalDocumentsNo",
      valueGetter: (params) => {
        const rejectedCount = params.row.rejectedDocumentsNo;
        const acceptedCount = params.row.acceptedDocumentsNo;
        return params.row.serviceTypeId == CertificateOfOriginId
          ? "-"
          : `${rejectedCount} Rejected; ${acceptedCount} Accepted of ${params.value}`;
      },
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          {params.row.serviceTypeId == CertificateOfOriginId ? (
            <Link
              to={{
                pathname: "/certificateOfOriginDetail",
                state: {
                  certificateOfOriginId: params.row.cerOfOriginId,
                  serviceTypeId: params.row.serviceTypeId,
                },
              }}
            >
              {" "}
              Detail
            </Link>
          ) : (
            <Link
              to={{
                pathname: "/serviceRequestDetail",
                state: {
                  serviceRequestId: params.value,
                  serviceTypeId: params.row.serviceTypeId,
                },
              }}
            >
              {" "}
              Detail
            </Link>
          )}
        </div>
      ),
    },
  ];
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={expanded}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div class="icon">
              <FontAwesomeIcon icon={faPaperPlane} />
            </div>
            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links"> Requested Services</p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            <Row>
              <Col md>
                <InputGroup>
                  <InputGroup.Text>Filter Status</InputGroup.Text>
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
              <Col md></Col>
            </Row>{" "}
            {loading ? (
              <h2 className="section-title">
                <br />
                Loading Requests list...
              </h2>
            ) : (
              <CustomDataGrid dataRows={serviceRequests} columns={columns} />
            )}
          </AccordionDetails>
        </Accordion>
      </Grid>
    </>
  );
}
