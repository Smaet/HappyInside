﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelPlay
{

    /// <summary>
    /// This behaviour should be attached to any object in the scene that can be recovered or damaged by the player
    /// </summary>
    public class Item : MonoBehaviour
    {

        /// <summary>
        /// The item type represented by this object.
        /// </summary>
        public ItemDefinition itemDefinition;

        public float quantity = 1f;

        public bool autoRotate = true;

        /// <summary>
        /// If set to true, this item will be added to the chunk items list. If item moves (ie. falls down), it will switch chunk automatically
        /// </summary>
        public bool persistentItem;

        /// <summary>
        /// If true, the object can be destroyed from the scene when picked up
        /// </summary>
        public bool canBeDestroyed;

        /// <summary>
        /// Resistance points left for this item. Used for items that can be damaged on the scene (not voxels).
        /// </summary>
        [NonSerialized]
        public byte resistancePointsLeft;

        /// <summary>
        /// If this object represents an item that can be picked
        /// </summary>
        public bool canPickOnApproach;

        [NonSerialized]
        public float creationTime;

        const float PICK_UP_START_DISTANCE_SQR = 6.5f;
        const float PICK_UP_END_DISTANCE_SQR = 0.81f;
        const float ROTATION_SPEED = 40f;

        [NonSerialized, HideInInspector]
        public Rigidbody rb;

        [NonSerialized]
        public bool pickingUp;

        VoxelPlayEnvironment env;
        VoxelChunk lastChunk;
        Material mat;
        Vector3d lastPosition;

        /// <summary>
        /// Stores the voxel related to this item (used atm for torches)
        /// </summary>
        public VoxelChunk itemChunk;
        public int itemVoxelIndex = -1;

        void Start ()
        {
            if (rb == null) {
                rb = GetComponent<Rigidbody> ();
            }
            env = VoxelPlayEnvironment.instance;
            if (persistentItem) {
                // Clone material to support voxel lighting
                Renderer renderer = GetComponent<Renderer> ();
                if (renderer != null) {
                    mat = renderer.sharedMaterial;
                    if (mat != null) {
                        mat = Instantiate<Material> (mat);
                        renderer.sharedMaterial = mat;
                    }
                }
                ManageItem ();
            }
        }

        void Update ()
        {
            if (this == null) return;

            if (transform.position.y < -11000) {
                // safety check in case the object drops to infinite
                Destroy (gameObject);
                return;
            }

            if (!canPickOnApproach || itemDefinition == null) {
                if (!pickingUp) {
                    return;
                }
            }

            if (autoRotate && rb != null) {
                rb.rotation = Quaternion.Euler (Misc.vector3up * ((Time.time * ROTATION_SPEED) % 360));
            }

            if (persistentItem) {
                ManageItem ();
            }

            if (!pickingUp) {
                if (Time.frameCount % 10 != 0)
                    return;
            }

            // Check if player is near
            Vector3 playerPosition = env.currentAnchorPosWS;
            Vector3 pos = transform.position;

            float dx = playerPosition.x - pos.x;
            float dy = playerPosition.y - pos.y;
            float dz = playerPosition.z - pos.z;

            if (pickingUp) {
                pos.x += dx * 0.25f;
                pos.y += dy * 0.25f;
                pos.z += dz * 0.25f;
                transform.position = pos;
            }

            if (Time.time - creationTime > 1f) {
                float dist = dx * dx + dy * dy + dz * dz;
                if (dist < PICK_UP_END_DISTANCE_SQR) {
                    IVoxelPlayPlayer player = VoxelPlayPlayer.instance;
                    if (player != null) {
                        player.PickUpItem (itemDefinition, quantity);
                    }
                    VoxelPlayUI ui = VoxelPlayUI.instance;
                    if (ui != null) {
                        ui.RefreshInventoryContents ();
                    }
                    if (persistentItem || canBeDestroyed) {
                        Destroy (gameObject);
                    } else {
                        gameObject.SetActive (false);
                    }
                } else if (dist < PICK_UP_START_DISTANCE_SQR) {
                    pickingUp = true;
                }
            }
        }

        public void PickItem ()
        {
            if (itemDefinition == null) return;
            if (!itemDefinition.canBePicked) return;
            pickingUp = true;
        }


        void ManageItem ()
        {
            if (env == null)
                return;

            Vector3d currentPosition = transform.position;

            if (lastChunk != null && lastChunk.isRendered) {
                if (currentPosition == lastPosition) {
                    return;
                }
                lastPosition = currentPosition;
            }

            // Update lighting
            if (mat != null) {
                float light = env.GetVoxelLightPacked (currentPosition);
                mat.SetFloat (ShaderParams.VoxelLight, light);
            }

            // Update owner chunk
            VoxelChunk currentChunk;
            if (!env.GetChunk (currentPosition, out currentChunk, true))
                return;
            if (currentChunk != lastChunk) {
                if (lastChunk != null) {
                    lastChunk.RemoveItem (this);
                    env.RegisterChunkChanges (lastChunk);
                }
                currentChunk.AddItem (this);
                env.RegisterChunkChanges (currentChunk);
                lastChunk = currentChunk;
            }

            if (currentChunk.isRendered && rb != null && !rb.useGravity) {
                rb.useGravity = true;
            }
        }

        void OnDestroy ()
        {
            if (lastChunk != null) {
                lastChunk.RemoveItem (this);
            }
        }


    }

}