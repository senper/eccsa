import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import { serviceTypes } from "variables/lookups.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";

import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import TextareaAutosize from "@material-ui/core/TextareaAutosize";
import MenuItem from "@material-ui/core/MenuItem";
import { Check, Edit, Info } from "@material-ui/icons";
import { Link, useLocation } from "react-router-dom";
import { Checkbox, FormControlLabel, FormLabel } from "@material-ui/core";
import CustomInput from "components/vender/CustomInput/CustomInput";
import Button from "components/vender/CustomButtons/Button.js";

import styles from "assets/jss/material-dashboard-pro-react/views/regularFormsStyle";
import { ASSOCIATION_DETAIL_QUERY } from "variables/associations_gql";
import { useQuery } from "urql";
import NavPills from "components/vender/NavPills/NavPills";
import { renderCellExpand } from "components/common/MaterialUIDataGrid/renderCellExpand";
import btnstyles from "assets/jss/material-dashboard-pro-react/views/buttonsStyle.js";
import CustomDataGrid from "components/common/MaterialUIDataGrid/CustomDataGrid";

const useStyles = makeStyles(styles);
const useBtnStyles = makeStyles(btnstyles);

export default function AssociationDetail() {
  const location = useLocation();
  const btnClasses = useBtnStyles();
  const [association, setAssociation] = React.useState(null);
  const [associationId, setAssociationId] = React.useState("");
  const [result, executeQuery] = useQuery({
    query: ASSOCIATION_DETAIL_QUERY,
    variables: { id: associationId },
    pause: true,
  });
  React.useEffect(() => {
    if (result.data) {
      const associationData = result.data.associationById;
      setAssociation(associationData);
      //console.log(association);
    }
  }, [result.data]);

  React.useEffect(() => {
    if (location.state) {
      setAssociationId(location.state.id);
      executeQuery();
    }
  }, [location, executeQuery]);

  const columns = [
    {
      headerName: "Name Eng",
      field: "nameEng",
      renderCell: renderCellExpand,
      flex: 0.3,
    },
    {
      headerName: "Name Local",
      field: "nameLocal",
      flex: 0.3,
    },
    {
      headerName: "Start Date",
      field: "startDate",
      flex: 0.08,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.08,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          {/* use this button to add a like kind of action */}
          <Button
            justIcon
            round
            color="primary"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Link
              style={{ color: "white" }}
              to={{
                pathname: location.state.layout + "/edit_board_member",
                state: { id: params.value },
              }}
            >
              <Edit className={btnClasses.icons} />
            </Link>
          </Button>
        </div>
      ),
    },
  ];
  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">
              <strong>
                {association
                  ? `${association.nameEng} (${association.nameLocal}) `
                  : null}
              </strong>
              {"Detail"}
            </CardIcon>
            <Link style={{ color: "orange" }} to="/membership/associations">
              {"<< back to list"}
            </Link>
          </CardHeader>
          <CardBody>
            {association ? (
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  Name:{" "}
                  <strong>
                    {association.nameEng} ({association.nameLocal})
                  </strong>
                  <br />
                </GridItem>
              </GridContainer>
            ) : null}
            <hr style={{ marginBottom: 0 }} />
            {association ? (
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  <NavPills
                    color="warning"
                    tabs={[
                      {
                        tabButton: "Board Members",
                        tabContent: (
                          <CustomDataGrid
                            dataRows={association.boardMembers}
                            columns={columns}
                          />
                        ),
                      },
                    ]}
                  />
                </GridItem>
              </GridContainer>
            ) : null}
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
