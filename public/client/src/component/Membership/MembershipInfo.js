import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Grid,
} from "@material-ui/core";
import { getMembershipInfo, getApplications } from "../../services/membership";
import ExistingMemberships from "./ExistingMemberships";
import ApplyForMembership from "./ApplyForMembership";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { Link } from "react-router-dom";

export default function MembershipInfo({ expanded = true }) {
  const token = localStorage.getItem("access_token");
  const [memberships, setMemberships] = React.useState([]);
  const [applications, setApplications] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    if (token) {
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
    }
  }, []);

  const columns = [
    {
      headerName: "Apply Date",
      field: "dateApplied",
      flex: 0.1,
    },
    {
      headerName: "Camber/Association",
      field: "associationName",
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Type",
      field: "applicationType",
      flex: 0.1,
    },
    {
      headerName: "Status",
      field: "applicationStatus",
      flex: 0.1,
    },
    {
      headerName: "Documents Status",
      field: "totalDocumentsNo",
      valueGetter: (params) => {
        const rejectedCount = params.row.rejectedDocumentsNo;
        const acceptedCount = params.row.acceptedDocumentsNo;
        return `${rejectedCount} Rejected; ${acceptedCount} Accepted of ${params.value}`;
      },
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.05,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            to={{
              pathname: "/membershipRequestDetail",
              state: {
                applicationId: params.value,
                associationId: params.row.associationId,
              },
            }}
          >
            {" "}
            Detail
          </Link>
          {/* {" / "}
          <Link
            to={{
              pathname: "/serviceRequestForm",
              state: {
                serviceRequestId: params.value,
                serviceTypeId: params.row.serviceTypeId,
              },
            }}
          >
            {" "}
            Edit
          </Link> */}
        </div>
      ),
    },
  ];
  return (
    <>
      {
        <style>
          {`.MuiAccordionSummary-content.Mui-expanded, .MuiAccordionSummary-content {margin: 0px;}
     .MuiAccordionSummary-root.Mui-expanded {min-height: 0px;}`}
        </style>
      }
      <Grid item xs={12}>
        <hr style={{ width: "100%", marginTop: "2px" }} />
        <h5>
          <p className="links" style={{ margin: "0px", textAlign: "left" }}>
            Existing Memberships
          </p>
        </h5>        
        {loading ? <h3>Loading...</h3> : null}
        <ExistingMemberships loading={loading} memberships={memberships} />
        <br />
        <ApplyForMembership memberships={memberships} />
        <Accordion defaultExpanded={expanded}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links"> Membership Requests</p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            {<CustomDataGrid dataRows={applications} columns={columns} />}
          </AccordionDetails>
        </Accordion>
      </Grid>
    </>
  );
}
